CREATE VIEW dbo.vwDistancePerBike
AS
SELECT        r.BikeId, SUM(COALESCE (r.GeoDistance, 0)) AS Expr1
FROM            dbo.rides AS r INNER JOIN
                         dbo.Bikes AS b ON r.BikeId = b.Id
GROUP BY r.BikeId
