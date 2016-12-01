CREATE view dbo.tempRides AS
SELECT [Id]
      ,[Duration]
      ,[Start]
      ,[Stop]
      ,[StartStationId]
      ,[EndStationId]
      ,[BikeId]
      ,[UserId]
      ,[EventType]
      ,case when Id % 400 = 0 then (SELECT ABS(CHECKSUM(NewId())) % 351 + 1) ELSE NULL END as EventId
      ,[GeoDistance]
  FROM [dbo].[rides]