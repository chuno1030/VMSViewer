CREATE TABLE TB_DeviceGroup(
	device_group_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	device_group_name VARCHAR(100) NOT NULL
);

CREATE TABLE TB_Device(
	device_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	device_group_id INT NOT NULL,
	device_name VARCHAR(100) NOT NULL,
	device_ip VARCHAR(50) NOT NULL,
	device_rtsp_address VARCHAR(200) NOT NULL
);