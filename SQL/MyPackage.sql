  CREATE OR REPLACE PACKAGE "KPH2012"."MYPACKAGE" 
as

type stringTableType is table of varchar2(20) index by binary_integer;
TYPE RefCursorType IS REF CURSOR;
procedure GetMyTableByIDs
(
p_MyIDList IN stringTableType,
p_outRefCursor out RefCursorType
);
procedure GetWaterObjectList
(
exp IN CHAR,
flowID in number,
c1 out SYS_REFCURSOR
);
end;

