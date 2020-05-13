use securitiesresearch;

#show tables;

#select sd.*, eod.* from endofdaydata eod
#join securitydetails sd on sd.Id = eod.SecurityId
#where sd.Symbol = 'AAPL'
#order by DateStamp desc;

#delete from endofdaydata where Id > 0;
#describe endofdaydata;

alter table endofdaydata
modify Open float;

alter table endofdaydata
modify High float;

alter table endofdaydata
modify Low float;

alter table endofdaydata
modify Close float;

alter table endofdaydata
modify AdjustedClose float;