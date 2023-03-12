CREATE TABLE temp.CompanyIds ("CompanyId" text NOT NULL);

CREATE TABLE temp.FeeConfigurationGroupImports (
    "DelayDays" int NULL,
    "Description" text NOT NULL,
    "Name" text NOT NULL,
    "Order" int NOT NULL,
    "Type" int NOT NULL
);

CREATE TABLE temp.FeeConfigurationImports (
    "CountryId" text NULL,
    "Order" int NOT NULL,
    "Type" int NOT NULL,
    "Value" decimal(18, 4) NOT NULL,
    "ValueMinimum" decimal(18, 4) NULL,
    "ValueType" int NOT NULL
);
