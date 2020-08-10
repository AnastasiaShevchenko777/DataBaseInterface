  CREATE OR REPLACE PACKAGE BODY "KPH2012"."MYPACKAGE" 
as

procedure GetMyTableByIDs
(
p_MyIDList IN stringTableType,
p_outRefCursor out RefCursorType
)
as
iMyIDList IDTableType;
begin

iMyIDList := IDTableType();
iMyIDList.Extend(p_MyIDList.count);

for i in p_MyIDList.First .. p_MyIDList.Last
loop
iMyIDList(i) := IDType(p_MyIDList(i));
end loop;

open p_outRefCursor
for
select kn,pnabl,priv1 from KPH_NEW where KPH_NEW.id in (select ID from table(iMyIDList));

end GetMyTableByIDs;

procedure GetWaterObjectList
(
exp IN CHAR,
flowID in number,
c1 out SYS_REFCURSOR
) is
BEGIN
open c1 for
select a.nwo, a.id from cod_wo a where a.exponent = exp and a.flows = flowID;
end GetWaterObjectList;

end;

/
