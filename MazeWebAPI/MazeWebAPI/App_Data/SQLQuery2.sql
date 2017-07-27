SELECT Users.Username, Users.id, COUNT(CASE Games.Winner WHEN Users.id THEN 1 ELSE null END) AS Wins, COUNT(Games.Id) AS NumOfGames
FROM Users LEFT OUTER JOIN Games
ON(Users.Id = Games.Player1ID OR Users.id = Games.Player2ID)
GROUP BY Users.Username, Users.Id
ORDER BY Wins DESC