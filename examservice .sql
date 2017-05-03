-- phpMyAdmin SQL Dump
-- version 4.3.11
-- http://www.phpmyadmin.net
--
-- Anamakine: 127.0.0.1
-- Üretim Zamanı: 03 May 2017, 14:20:01
-- Sunucu sürümü: 5.6.24
-- PHP Sürümü: 5.6.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Veritabanı: `examservice`
--

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `exams`
--

CREATE TABLE IF NOT EXISTS `exams` (
  `Id` varchar(255) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `LessonId` int(11) NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `Questions` text,
  `Title` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `lessons`
--

CREATE TABLE IF NOT EXISTS `lessons` (
  `Id` int(11) NOT NULL,
  `Akts` int(11) NOT NULL,
  `Code` varchar(255) DEFAULT NULL,
  `Delete` bit(1) NOT NULL DEFAULT b'0',
  `Guid` varchar(255) DEFAULT NULL,
  `Hours` int(11) NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `UserId` varchar(255) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;


--
-- Tablo için tablo yapısı `questionpools`
--

CREATE TABLE IF NOT EXISTS `questionpools` (
  `Id` varchar(255) NOT NULL,
  `Answer` text CHARACTER SET utf8 COLLATE utf8_bin,
  `Delete` bit(1) NOT NULL DEFAULT b'0',
  `Description` text CHARACTER SET utf8 COLLATE utf8_bin,
  `ExamFormat` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `ExamType` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `LessonId` int(11) NOT NULL,
  `Question` text CHARACTER SET utf8 COLLATE utf8_bin,
  `SubjectId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `roleclaims`
--

CREATE TABLE IF NOT EXISTS `roleclaims` (
  `Id` int(11) NOT NULL,
  `ClaimType` varchar(255) DEFAULT NULL,
  `ClaimValue` varchar(255) DEFAULT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `roles`
--

CREATE TABLE IF NOT EXISTS `roles` (
  `Id` varchar(255) NOT NULL,
  `ConcurrencyStamp` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `NormalizedName` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `subjects`
--

CREATE TABLE IF NOT EXISTS `subjects` (
  `Id` int(11) NOT NULL,
  `Delete` bit(1) NOT NULL DEFAULT b'0',
  `LessonId` int(11) NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;


-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `userclaims`
--

CREATE TABLE IF NOT EXISTS `userclaims` (
  `Id` int(11) NOT NULL,
  `ClaimType` varchar(255) DEFAULT NULL,
  `ClaimValue` varchar(255) DEFAULT NULL,
  `UserId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `userroles`
--

CREATE TABLE IF NOT EXISTS `userroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `users`
--

CREATE TABLE IF NOT EXISTS `users` (
  `Id` varchar(255) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `ConcurrencyStamp` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `EmailConfirmed` bit(1) NOT NULL,
  `LockoutEnabled` bit(1) NOT NULL,
  `LockoutEnd` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `NormalizedEmail` varchar(255) DEFAULT NULL,
  `NormalizedUserName` varchar(255) DEFAULT NULL,
  `PasswordHash` varchar(255) DEFAULT NULL,
  `PhoneNumber` varchar(255) DEFAULT NULL,
  `PhoneNumberConfirmed` bit(1) NOT NULL,
  `SecurityStamp` varchar(255) DEFAULT NULL,
  `TwoFactorEnabled` bit(1) NOT NULL,
  `UserName` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `__efmigrationshistory`
--

CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8 NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Dökümü yapılmış tablolar için indeksler
--

--
-- Tablo için indeksler `exams`
--
ALTER TABLE `exams`
  ADD PRIMARY KEY (`Id`), ADD KEY `IX_Exams_LessonId` (`LessonId`);

--
-- Tablo için indeksler `lessons`
--
ALTER TABLE `lessons`
  ADD PRIMARY KEY (`Id`), ADD KEY `IX_Lessons_UserId` (`UserId`);

--
-- Tablo için indeksler `questionpools`
--
ALTER TABLE `questionpools`
  ADD PRIMARY KEY (`Id`), ADD KEY `IX_QuestionPools_LessonId` (`LessonId`), ADD KEY `IX_QuestionPools_SubjectId` (`SubjectId`);

--
-- Tablo için indeksler `roleclaims`
--
ALTER TABLE `roleclaims`
  ADD PRIMARY KEY (`Id`), ADD KEY `IX_RoleClaims_RoleId` (`RoleId`);

--
-- Tablo için indeksler `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`Id`), ADD KEY `RoleNameIndex` (`NormalizedName`);

--
-- Tablo için indeksler `subjects`
--
ALTER TABLE `subjects`
  ADD PRIMARY KEY (`Id`), ADD KEY `IX_Subjects_LessonId` (`LessonId`);

--
-- Tablo için indeksler `userclaims`
--
ALTER TABLE `userclaims`
  ADD PRIMARY KEY (`Id`), ADD KEY `IX_UserClaims_UserId` (`UserId`);

--
-- Tablo için indeksler `userroles`
--
ALTER TABLE `userroles`
  ADD PRIMARY KEY (`UserId`,`RoleId`), ADD KEY `IX_UserRoles_RoleId` (`RoleId`), ADD KEY `IX_UserRoles_UserId` (`UserId`);

--
-- Tablo için indeksler `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`), ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`), ADD KEY `EmailIndex` (`NormalizedEmail`);

--
-- Tablo için indeksler `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- Dökümü yapılmış tablolar için AUTO_INCREMENT değeri
--

--
-- Tablo için AUTO_INCREMENT değeri `lessons`
--
ALTER TABLE `lessons`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
--
-- Tablo için AUTO_INCREMENT değeri `roleclaims`
--
ALTER TABLE `roleclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- Tablo için AUTO_INCREMENT değeri `subjects`
--
ALTER TABLE `subjects`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=6;
--
-- Tablo için AUTO_INCREMENT değeri `userclaims`
--
ALTER TABLE `userclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;
--
-- Dökümü yapılmış tablolar için kısıtlamalar
--

--
-- Tablo kısıtlamaları `exams`
--
ALTER TABLE `exams`
ADD CONSTRAINT `FK_Exams_Lessons_LessonId` FOREIGN KEY (`LessonId`) REFERENCES `lessons` (`Id`) ON DELETE CASCADE;

--
-- Tablo kısıtlamaları `lessons`
--
ALTER TABLE `lessons`
ADD CONSTRAINT `FK_Lessons_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`);

--
-- Tablo kısıtlamaları `questionpools`
--
ALTER TABLE `questionpools`
ADD CONSTRAINT `FK_QuestionPools_Lessons_LessonId` FOREIGN KEY (`LessonId`) REFERENCES `lessons` (`Id`) ON DELETE CASCADE,
ADD CONSTRAINT `FK_QuestionPools_Subjects_SubjectId` FOREIGN KEY (`SubjectId`) REFERENCES `subjects` (`Id`) ON DELETE CASCADE;

--
-- Tablo kısıtlamaları `roleclaims`
--
ALTER TABLE `roleclaims`
ADD CONSTRAINT `FK_RoleClaims_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE;

--
-- Tablo kısıtlamaları `subjects`
--
ALTER TABLE `subjects`
ADD CONSTRAINT `FK_Subjects_Lessons_LessonId` FOREIGN KEY (`LessonId`) REFERENCES `lessons` (`Id`) ON DELETE CASCADE;

--
-- Tablo kısıtlamaları `userclaims`
--
ALTER TABLE `userclaims`
ADD CONSTRAINT `FK_UserClaims_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Tablo kısıtlamaları `userroles`
--
ALTER TABLE `userroles`
ADD CONSTRAINT `FK_UserRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE,
ADD CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
