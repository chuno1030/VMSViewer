create table TB_Client(
	client_id int not null primary key auto_increment,
	group_id int not null,
	client_name varchar(300) not null,
	client_ip varchar(100) not null,
	rtsp_address varchar(500) not null
);
