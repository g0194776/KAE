/*
SQLyog Ultimate v11.13 (64 bit)
MySQL - 5.5.18-log : Database - csndb
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`csndb` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_bin */;

USE `csndb`;

/*Table structure for table `ha_configinfo` */

DROP TABLE IF EXISTS `ha_configinfo`;

CREATE TABLE `ha_configinfo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ServiceName` varchar(64) DEFAULT NULL,
  `ConfigKey` varchar(64) DEFAULT NULL,
  `ConfigValue` varchar(256) DEFAULT NULL,
  `CreateTime` timestamp NOT NULL DEFAULT '2011-12-12 00:00:00',
  `LastOprTime` timestamp NOT NULL DEFAULT '2011-12-12 00:00:00',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=665 DEFAULT CHARSET=utf8;

/*Table structure for table `ha_PartialConfig` */

DROP TABLE IF EXISTS `ha_PartialConfig`;

CREATE TABLE `ha_PartialConfig` (
  `ConfigKey` varchar(128) COLLATE utf8_bin NOT NULL,
  `ConfigValue` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

/*Table structure for table `ha_serviceinfo` */

DROP TABLE IF EXISTS `ha_serviceinfo`;

CREATE TABLE `ha_serviceinfo` (
  `ServiceId` int(11) NOT NULL AUTO_INCREMENT,
  `ServiceName` varchar(64) DEFAULT NULL,
  `Range` int(11) DEFAULT NULL,
  `CreateTime` timestamp NOT NULL DEFAULT '2011-12-12 00:00:00',
  `LastOprTime` timestamp NOT NULL DEFAULT '2011-12-12 00:00:00',
  PRIMARY KEY (`ServiceId`)
) ENGINE=MyISAM AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

/*Table structure for table `ha_serviceroutetable` */

DROP TABLE IF EXISTS `ha_serviceroutetable`;

CREATE TABLE `ha_serviceroutetable` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ServiceId` int(11) DEFAULT NULL,
  `ServiceAddress` varchar(32) DEFAULT NULL,
  `BeginBoundary` smallint(6) DEFAULT NULL,
  `EndBoundary` smallint(6) DEFAULT NULL,
  `CreateTime` timestamp NOT NULL DEFAULT '2011-12-12 00:00:00',
  `LastOprTime` timestamp NOT NULL DEFAULT '2011-12-12 00:00:00',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=39 DEFAULT CHARSET=utf8;

/* Procedure structure for procedure `UPS_ExcuteDynamicProc` */

/*!50003 DROP PROCEDURE IF EXISTS  `UPS_ExcuteDynamicProc` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `UPS_ExcuteDynamicProc`(
p_TableName varchar(100))
begin
	set @STMT = Concat('SELECT * FROM ',p_TableName);
	PREPARE STMT from @STMT;
	EXECUTE STMT;
	DEALLOCATE PREPARE STMT;
end */$$
DELIMITER ;

/* Procedure structure for procedure `UPS_ModifyConfig` */

/*!50003 DROP PROCEDURE IF EXISTS  `UPS_ModifyConfig` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `UPS_ModifyConfig`(p_Id int,p_Value varchar(256))
BEGIN
	update `ha_configinfo` set `ConfigValue` = p_Value where `Id` = p_Id;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `USP_GetConfigInfo` */

/*!50003 DROP PROCEDURE IF EXISTS  `USP_GetConfigInfo` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `USP_GetConfigInfo`(
p_serviceName varchar(64)
)
BEGIN
	SELECT * FROM ha_configinfo where ServiceName = p_ServiceName;
END */$$
DELIMITER ;

/* Procedure structure for procedure `USP_GetConfigInfoItemByServiceName` */

/*!50003 DROP PROCEDURE IF EXISTS  `USP_GetConfigInfoItemByServiceName` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `USP_GetConfigInfoItemByServiceName`(
p_ServiceName varchar(64)
)
BEGIN
	SELECT * FROM ha_configinfo where ServiceName = p_ServiceName;
END */$$
DELIMITER ;

/* Procedure structure for procedure `USP_GetPartialConfig` */

/*!50003 DROP PROCEDURE IF EXISTS  `USP_GetPartialConfig` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `USP_GetPartialConfig`(
	p_ConfigKey varchar(1024))
BEGIN
	select `ConfigValue` from `csndb`.`ha_PartialConfig`
	where `ConfigKey` = p_ConfigKey
	limit 1;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `USP_GetServiceInfo` */

/*!50003 DROP PROCEDURE IF EXISTS  `USP_GetServiceInfo` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `USP_GetServiceInfo`()
BEGIN	SELECT * FROM ha_serviceinfo;END */$$
DELIMITER ;

/* Procedure structure for procedure `USP_GetServiceRouteTable` */

/*!50003 DROP PROCEDURE IF EXISTS  `USP_GetServiceRouteTable` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `USP_GetServiceRouteTable`(
)
BEGIN
	SELECT ServiceName,ServiceAddress,BeginBoundary,EndBoundary
	FROM ha_serviceroutetable a
	INNER JOIN ha_serviceinfo b
	ON a.ServiceId = b.ServiceId;
END */$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
