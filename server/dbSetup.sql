CREATE TABLE IF NOT EXISTS accounts (
    id VARCHAR(255) NOT NULL PRIMARY KEY COMMENT 'primary key',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    name VARCHAR(255) COMMENT 'User Name',
    email VARCHAR(255) UNIQUE COMMENT 'User Email',
    picture VARCHAR(255) COMMENT 'User Picture'
) default charset utf8mb4 COMMENT '';

CREATE TABLE recipes (
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created' NOT NULL,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update' NOT NULL,
    title VARCHAR(255) NOT NULL,
    instructions VARCHAR(1000),
    img VARCHAR(1000) NOT NULL,
    category ENUM(
        "breakfast",
        "lunch",
        "dinner",
        "snack",
        "dessert"
    ) NOT NULL,
    creator_id VARCHAR(255) NOT NULL,
    FOREIGN KEY (creator_id) REFERENCES accounts (id) ON DELETE CASCADE
) default charset utf8mb4 COMMENT '';

INSERT INTO
    recipes (
        title,
        instructions,
        img,
        category,
        creator_id
    )
VALUES (
        @title,
        @instructions,
        @img,
        @category,
        @creator_id
    );

CREATE TABLE ingredients (
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created' NOT NULL,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update' NOT NULL,
    name VARCHAR(255) NOT NULL,
    quantity VARCHAR(255) NOT NULL,
    recipe_id INT NOT NULL,
    Foreign Key (recipe_id) REFERENCES recipes (id) ON DELETE CASCADE
) default charset utf8mb4 COMMENT '';

INSERT INTO
    ingredients (name, quantity, recipe_id)
VALUES ("celery", "1 stick", 1);

CREATE TABLE favorites (
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created' NOT NULL,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update' NOT NULL,
    recipe_id INT NOT NULL,
    account_id VARCHAR(255) NOT NULL,
    FOREIGN KEY (recipe_id) REFERENCES recipes (id) ON DELETE CASCADE,
    FOREIGN KEY (account_id) REFERENCES accounts (id) ON DELETE CASCADE
) default charset utf8mb4 COMMENT '';

SELECT recipes.*, favorites.id AS favoriteId, accounts.*
FROM
    favorites
    JOIN recipes ON recipes.id = favorites.recipe_id
    JOIN accounts ON accounts.id = recipes.creator_id
WHERE
    favorites.id = 2;

SELECT recipes.*, COUNT(favorites.id) AS favoritesCount, accounts.*
FROM
    recipes
    LEFT JOIN favorites ON recipes.id = favorites.recipe_id
    JOIN accounts ON recipes.creator_id = accounts.id
GROUP BY
    recipes.id;

SELECT recipes.*, COUNT(favorites.id) AS favoritesCount, accounts.*
FROM
    recipes
    JOIN accounts ON recipes.creator_id = accounts.id
    LEFT JOIN favorites ON recipes.id = favorites.recipe_id
GROUP BY
    recipes.id
ORDER BY recipes.id

SELECT recipes.*, favorites.*, accounts.*
FROM
    recipes
    JOIN accounts ON recipes.creator_id = accounts.id
    LEFT JOIN favorites ON recipes.id = favorites.recipe_id
WHERE
    favorites.account_id = @userId;

SELECT recipes.*, favorites.*, accounts.*
FROM
    favorites
    left JOIN recipes ON favorites.recipe_id = recipes.id
    JOIN accounts ON recipes.creator_id = accounts.id

SELECT recipes.*, COUNT(favorites.id) AS favoriteCount, accounts.*
FROM
    recipes
    JOIN accounts ON recipes.creator_id = accounts.id
    JOIN favorites ON recipes.id = favorites.recipe_id
GROUP BY
    favorites.recipe_id
ORDER BY recipes.id ASC

SELECT
    recipes.*,
    COUNT(favorites.id) AS favoriteCount,
    favorites.id AS favoriteId,
    favorites.account_id AS accountId,
    accounts.*
FROM
    favorites
    RIGHT JOIN recipes ON favorites.recipe_id = recipes.id
    JOIN accounts ON recipes.creator_id = accounts.id
GROUP BY
    recipes.id

CREATE VIEW recipes_with_fav_count AS
SELECT recipes.*, COUNT(favorites.recipe_id) AS favoriteCount
FROM recipes
    LEFT JOIN favorites ON recipes.id = favorites.recipe_id
GROUP BY
    recipes.id
ORDER BY recipes.id;

SELECT recipes.*, accounts.*
FROM
    recipes_with_fav_count AS recipes
    JOIN accounts ON recipes.creator_id = accounts.id
    LEFT JOIN favorites ON recipes.id = favorites.recipe_id
GROUP BY
    recipes.id
ORDER BY recipes.id ASC;