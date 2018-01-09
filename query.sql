select distinct FuncVer from FunctionRun

select 
	b.FuncVer,
	a.MessageKey,
	b.Step,
	CASE WHEN b.Step=1 THEN datediff(ms, a.InsertDate, b.InsertDate) ELSE null END as PickupDuration,
	CASE WHEN b.Step=2 THEN datediff(ms, a.InsertDate, b.InsertDate) ELSE null END as Pickup_PLUS_Run_Duration
from
	BusMessage a
	left outer join FunctionRun b on a.MessageKey = b.RunKey
order by
	a.InsertDate

select 
	a.MessageKey as MISSING
from
	BusMessage a
	left outer join FunctionRun b on a.MessageKey = b.RunKey
where
	b.RunKey is null

select 
	a.MessageKey as TOO_MANY,
	count(b.RunKey) as Should_Be_2
from
	BusMessage a
	left outer join FunctionRun b on a.MessageKey = b.RunKey
group by
	a.MessageKey
having
	count(b.RunKey)>2


select 
	a.MessageKey as NOT_ENOUGH,
	count(b.RunKey) as Should_Be_2
from
	BusMessage a
	left outer join FunctionRun b on a.MessageKey = b.RunKey
group by
	a.MessageKey
having
	count(b.RunKey)<2
	
select distinct
	b.RunKey as I_DID_NOT_SEND
from
	FunctionRun b
	left outer join BusMessage a on b.RunKey = a.MessageKey
where
	a.MessageKey is null

select distinct ProcessId,ThreadId from FunctionRun
select * from FunctionException


/*

select * from BusMessage
select * from FunctionRun
select * from FunctionException

delete from BusMessage
delete from FunctionRun
delete from FunctionException

select 
	b.FuncVer,
	a.MessageKey,
	b.Step,
	CASE WHEN b.Step=1 THEN datediff(ms, a.InsertDate, b.InsertDate) ELSE null END as PickupDuration,
	CASE WHEN b.Step=2 THEN datediff(ms, a.InsertDate, b.InsertDate) ELSE null END as RunDuration
from
	BusMessage a
	left outer join FunctionRun b on a.MessageKey = b.RunKey
where
	a.MessageKey='590fd04f-da38-48d6-a67d-a4ffd2f6aa3a'
order by
	b.InsertDate


*/
