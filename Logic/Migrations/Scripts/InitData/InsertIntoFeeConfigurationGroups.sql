-- noinspection SqlResolveForFile

INSERT INTO FeeConfigurationGroups
(
    "CompanyId",
    "DelayDays",
    "Description",
    "Name",
    "Order",
    "Type"
)
SELECT
    "CompanyId",
    "DelayDays",
    "Description",
    "Name",
    "Order",
    "Type"
FROM temp.FeeConfigurationGroupImports
CROSS JOIN temp.CompanyIds;
