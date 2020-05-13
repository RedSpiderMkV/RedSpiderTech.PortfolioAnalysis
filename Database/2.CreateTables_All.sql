use SecuritiesResearch;

create table if not exists ExchangeDetails
(
	Id int not null auto_increment,
    Name varchar(100),
    primary key (Id)
);

create table if not exists SecurityDetails
(
	Id int not null auto_increment,
    Name varchar(255),
    Symbol varchar(10),
    ExchangeId int,
    primary key (Id),
    foreign key(ExchangeId) references ExchangeDetails(Id)
);

create table VendorDetails
(
	Id int,
    Name varchar(20),
    primary key(Id)
);

create table if not exists EndOfDayData
(
	Id int not null auto_increment,
	SecurityId int not null,
    VendorId int not null,
    Open float,
    High float,
    Low float,
    Close float,
    AdjustedClose float,
    Volume bigint,
    DayChange float,
    DayPercentageChange float,
    StandardChange float,
    StandardPercentageChange float,
    DateStamp date,
    primary key (Id),
    foreign key(SecurityId) references SecurityDetails(Id),
    foreign key(VendorId) references VendorDetails(Id)
);
