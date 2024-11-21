
# PostgreSQL: Реализация базы данных «Библиотека» и запросы

## Схема таблиц

![Пример изображения](images/схема%20таблиц.png)

### Таблица `Authors` (Авторы)
```sql
CREATE TABLE Authors (
    AuthorId SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
);
```

### Таблица `Books` (Книги)
```sql
CREATE TABLE Books (
    BookId SERIAL PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    AuthorId INT NOT NULL,
    PublishedYear INT,
    FOREIGN KEY (AuthorId) REFERENCES Authors(AuthorId)
);
```

### Таблица `Readers` (Читатели)
```sql
CREATE TABLE Readers (
    ReaderId SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255)
);
```

### Таблица `BookIssues` (Выдача книг)
```sql
CREATE TABLE BookIssues (
    IssueId SERIAL PRIMARY KEY,
    BookId INT NOT NULL,
    ReaderId INT NOT NULL,
    IssueDate DATE NOT NULL,
    ReturnDate DATE,
    FOREIGN KEY (BookId) REFERENCES Books(BookId),
    FOREIGN KEY (ReaderId) REFERENCES Readers(ReaderId)
);
```

---

## Запросы

### 2. Запрос: Все читатели с более чем одной книгой на руках
```sql
SELECT 
    r.ReaderId,
    r.Name,
    COUNT(bi.BookId) AS BookCount
FROM 
    Readers r
JOIN 
    BookIssues bi ON r.ReaderId = bi.ReaderId
WHERE 
    bi.ReturnDate IS NULL
GROUP BY 
    r.ReaderId, r.Name
HAVING 
    COUNT(bi.BookId) > 1;
```

---

### 3. Запрос: Авторы с количеством книг выше среднего, отсортированные по убыванию
```sql
WITH AuthorBookCounts AS (
    SELECT 
        a.AuthorId,
        a.Name,
        COUNT(b.BookId) AS BookCount
    FROM 
        Authors a
    JOIN 
        Books b ON a.AuthorId = b.AuthorId
    GROUP BY 
        a.AuthorId, a.Name
)
SELECT 
    AuthorId,
    Name,
    BookCount
FROM 
    AuthorBookCounts
WHERE 
    BookCount > (SELECT AVG(BookCount) FROM AuthorBookCounts)
ORDER BY 
    BookCount DESC;
```

---

### 4. Запрос: Все книги, которые сейчас не у читателей
```sql
SELECT 
    b.BookId,
    b.Title
FROM 
    Books b
LEFT JOIN 
    BookIssues bi ON b.BookId = bi.BookId AND bi.ReturnDate IS NULL
WHERE 
    bi.BookId IS NULL;
```

---

### 5. Запрос: Все книги, которые были у читателей 01.01.2022
```sql
SELECT 
    b.BookId,
    b.Title,
    r.Name AS ReaderName
FROM 
    Books b
JOIN 
    BookIssues bi ON b.BookId = bi.BookId
JOIN 
    Readers r ON bi.ReaderId = r.ReaderId
WHERE 
    '2022-01-01' BETWEEN bi.IssueDate AND COALESCE(bi.ReturnDate, '2022-01-01');
```
