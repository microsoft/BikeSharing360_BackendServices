CREATE VIEW dbo.tempEventRating AS
SELECT DISTINCT UserId 
	, eve.Name AS EventName
	, 5.0 AS Rating 
FROM dbo.temprides rid
	JOIN dbo.events eve on rid.EventId = eve.Id