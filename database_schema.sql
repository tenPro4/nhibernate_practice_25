-- Create Department table
CREATE TABLE `Department` (
    `Id` INT AUTO_INCREMENT NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

-- Create Employee table with inheritance
CREATE TABLE `Employee` (
    `Id` INT AUTO_INCREMENT NOT NULL,
    `FirstName` VARCHAR(50) NOT NULL,
    `LastName` VARCHAR(50) NOT NULL,
    `Salary` DECIMAL(10,2) NOT NULL,
    `DepartmentId` INT NOT NULL,
    `EmployeeType` VARCHAR(50) NOT NULL,
    `VacationDays` INT NULL,           -- For FullTimeEmployee
    `InsuranceNumber` VARCHAR(50) NULL, -- For FullTimeEmployee
    `ContractEndDate` DATETIME NULL,    -- For ContractEmployee
    `HourlyRate` DECIMAL(10,2) NULL,    -- For ContractEmployee
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Employee_Department` FOREIGN KEY (`DepartmentId`) 
        REFERENCES `Department` (`Id`)
) ENGINE=InnoDB;

-- Create indexes
CREATE INDEX `IDX_Employee_DepartmentId` ON `Employee` (`DepartmentId`);
CREATE INDEX `IDX_Employee_EmployeeType` ON `Employee` (`EmployeeType`); 