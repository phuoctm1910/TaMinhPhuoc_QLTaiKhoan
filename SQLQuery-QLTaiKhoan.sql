create database QLTaiKhoan
go
use QLTaiKhoan
go

create table Role(
	roleID int identity(1,1) primary key,
	roleName nvarchar(25) not null
)
go

insert into Role
values('Admin'),('Employ'),('User')
go

create table Account(
	userName varchar(20) not null primary key,
	ownerName nvarchar(25) not null,
	email varchar(20) not null Unique,
	passWord varchar(50) not null,
	roleID int not null,
	constraint FK_Role_Account foreign key(roleID)
	references Role(roleID)
)
go

insert into Account
values('admin', 'Tạ Minh Phước', 'phuoctm@gmail.com', '123456', 1),
('employ', 'Hải Vân', 'vanlth@gmail.com', '123456', 2)
go