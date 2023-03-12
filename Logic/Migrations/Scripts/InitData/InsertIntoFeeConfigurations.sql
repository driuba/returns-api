-- noinspection SqlResolveForFile

INSERT INTO FeeConfigurations
(
    "CountryId",
    "Deleted",
    "FeeConfigurationGroupId",
    "Value",
    "ValueMinimum",
    "ValueType",
    "Created",
    "Modified",
    "UserCreated",
    "UserModified"
)
SELECT
    fci."CountryId",
    0,
    fcg."Id",
    fci."Value",
    fci."ValueMinimum",
    fci."ValueType",
    datetime('now', 'localtime'),
    NULL,
    'Migration',
    NULL
FROM FeeConfigurationGroups fcg
INNER JOIN temp.FeeConfigurationImports fci ON
    fci."Order" = fcg."Order" AND
    fci."Type" = fcg."Type";
