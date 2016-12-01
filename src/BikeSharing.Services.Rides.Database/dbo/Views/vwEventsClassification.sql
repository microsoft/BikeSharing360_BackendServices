CREATE VIEW dbo.[vwEventsClassification] AS
SELECT DISTINCT eve.Name AS EventName
	, seg.Name + '|' + gen.Name + '|' + sub.Name AS Classification
FROM dbo.events eve
	join dbo.Classifications gen on eve.GenreId = gen.Id
	join dbo.Classifications sub on eve.SubGenreId = sub.Id
	join dbo.Classifications seg on eve.SegmentId = seg.Id