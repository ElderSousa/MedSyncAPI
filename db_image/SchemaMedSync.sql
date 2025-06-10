-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: medsync
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `agendamentos`
--

DROP TABLE IF EXISTS `agendamentos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agendamentos` (
  `Id` char(36) NOT NULL,
  `AgendaId` char(36) NOT NULL,
  `MedicoId` char(36) NOT NULL,
  `PacienteId` char(36) NOT NULL,
  `AgendadoPara` datetime NOT NULL,
  `Horario` time NOT NULL,
  `DiaSemana` tinyint NOT NULL,
  `Tipo` tinyint NOT NULL,
  `Status` tinyint NOT NULL,
  `CriadoEm` datetime NOT NULL,
  `CriadoPor` char(36) DEFAULT NULL,
  `ModificadoEm` datetime DEFAULT NULL,
  `ModificadoPor` char(36) DEFAULT NULL,
  `ExcluidoEm` datetime DEFAULT NULL,
  `agendamentoscol` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_agendamento_agenda` (`AgendaId`),
  KEY `fk_agendamento_Medico` (`MedicoId`),
  KEY `fk_agendamento_paciente` (`PacienteId`),
  CONSTRAINT `fk_agendamento_agenda` FOREIGN KEY (`AgendaId`) REFERENCES `agendas` (`Id`),
  CONSTRAINT `fk_agendamento_Medico` FOREIGN KEY (`MedicoId`) REFERENCES `medicos` (`Id`),
  CONSTRAINT `fk_agendamento_paciente` FOREIGN KEY (`PacienteId`) REFERENCES `pacientes` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `agendas`
--

DROP TABLE IF EXISTS `agendas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agendas` (
  `Id` char(36) NOT NULL,
  `MedicoId` char(36) NOT NULL,
  `DataDisponivel` date NOT NULL,
  `DiaSemana` tinyint NOT NULL DEFAULT '0',
  `CriadoEm` datetime NOT NULL,
  `CriadoPor` char(36) DEFAULT NULL,
  `ModificadoEm` datetime DEFAULT NULL,
  `ModificadoPor` char(36) DEFAULT NULL,
  `ExcluidoEm` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `MedicoId_idx` (`MedicoId`),
  CONSTRAINT `fk_Agenda_medico` FOREIGN KEY (`MedicoId`) REFERENCES `medicos` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `enderecos`
--

DROP TABLE IF EXISTS `enderecos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `enderecos` (
  `Id` char(36) NOT NULL,
  `PacienteId` char(36) DEFAULT NULL,
  `MedicoId` char(36) DEFAULT NULL,
  `Logradouro` varchar(150) NOT NULL,
  `Numero` varchar(10) NOT NULL,
  `Complemento` varchar(50) DEFAULT NULL,
  `Bairro` varchar(100) NOT NULL,
  `Cidade` varchar(100) NOT NULL,
  `Estado` varchar(2) NOT NULL,
  `CEP` varchar(10) DEFAULT NULL,
  `CriadoEm` datetime NOT NULL,
  `CriadoPor` char(36) DEFAULT NULL,
  `ModificadoEm` datetime DEFAULT NULL,
  `ModificadoPor` char(36) DEFAULT NULL,
  `ExcluidoEm` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Enderecos_pacientes_idx` (`PacienteId`),
  KEY `fk_Enderecos_medicos_idx` (`MedicoId`),
  CONSTRAINT `fk_Endereco_medico` FOREIGN KEY (`MedicoId`) REFERENCES `medicos` (`Id`),
  CONSTRAINT `fk_Endereco_paciente` FOREIGN KEY (`PacienteId`) REFERENCES `pacientes` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `horarios`
--

DROP TABLE IF EXISTS `horarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `horarios` (
  `Id` char(36) NOT NULL,
  `AgendaId` char(36) NOT NULL,
  `Hora` time NOT NULL,
  `Agendado` tinyint DEFAULT '0',
  `CriadoEm` datetime NOT NULL,
  `CriadoPor` char(36) DEFAULT NULL,
  `ModificadoEm` datetime DEFAULT NULL,
  `ModificadoPor` char(36) DEFAULT NULL,
  `ExcluidoEm` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_horario_agenda` (`AgendaId`),
  CONSTRAINT `fk_horario_agenda` FOREIGN KEY (`AgendaId`) REFERENCES `agendas` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `medicos`
--

DROP TABLE IF EXISTS `medicos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medicos` (
  `Id` char(36) NOT NULL,
  `PessoaId` char(36) NOT NULL,
  `CRM` varchar(20) NOT NULL,
  `Especialidade` tinyint NOT NULL DEFAULT '0',
  `CriadoEm` datetime NOT NULL,
  `CriadoPor` char(36) DEFAULT NULL,
  `ModificadoEm` datetime DEFAULT NULL,
  `ModificadoPor` char(36) DEFAULT NULL,
  `ExcluidoEm` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_medico_pessoa_idx` (`PessoaId`),
  CONSTRAINT `fk_Medico_pessoa` FOREIGN KEY (`PessoaId`) REFERENCES `pessoas` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pacientes`
--

DROP TABLE IF EXISTS `pacientes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pacientes` (
  `Id` char(36) NOT NULL,
  `PessoaId` char(36) NOT NULL,
  `CriadoEm` datetime NOT NULL,
  `CriadoPor` char(36) DEFAULT NULL,
  `ModificadoEm` datetime DEFAULT NULL,
  `ModificadoPor` char(36) DEFAULT NULL,
  `ExcluidoEm` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Pacientes_pessoas1_idx` (`PessoaId`),
  CONSTRAINT `fk_paciente_pessoa` FOREIGN KEY (`PessoaId`) REFERENCES `pessoas` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pessoas`
--

DROP TABLE IF EXISTS `pessoas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pessoas` (
  `Id` char(36) NOT NULL,
  `Nome` char(100) NOT NULL,
  `CPF` varchar(14) NOT NULL,
  `RG` varchar(20) NOT NULL,
  `DataNascimento` date NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Sexo` varchar(1) DEFAULT NULL,
  `CriadoEm` datetime NOT NULL,
  `CriadoPor` char(36) DEFAULT NULL,
  `ModificadoEm` datetime DEFAULT NULL,
  `ModificadoPor` char(36) DEFAULT NULL,
  `ExcluidoEm` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `telefones`
--

DROP TABLE IF EXISTS `telefones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `telefones` (
  `Id` char(36) NOT NULL,
  `PacienteId` char(36) DEFAULT NULL,
  `MedicoId` char(36) DEFAULT NULL,
  `Numero` char(15) NOT NULL,
  `Tipo` tinyint NOT NULL DEFAULT '0',
  `CriadoEm` datetime NOT NULL,
  `CriadoPor` char(36) DEFAULT NULL,
  `ModificadoEm` datetime DEFAULT NULL,
  `ModificadoPor` char(36) DEFAULT NULL,
  `ExcluidoEm` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Telefones_pacientes_idx` (`PacienteId`),
  KEY `fk_Telefones_medicos_idx` (`MedicoId`),
  CONSTRAINT `fk_Telefone_medico` FOREIGN KEY (`MedicoId`) REFERENCES `medicos` (`Id`),
  CONSTRAINT `fk_Telefone_paciente` FOREIGN KEY (`PacienteId`) REFERENCES `pacientes` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-09 10:41:50
