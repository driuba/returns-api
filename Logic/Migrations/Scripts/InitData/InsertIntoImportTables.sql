-- noinspection SqlResolveForFile

INSERT INTO temp.CompanyIds(CompanyId)
VALUES
    ('aaa'),
    ('bbb');

INSERT INTO temp.FeeConfigurationGroupImports
(
    "DelayDays",
    "Description",
    "Name",
    "Order",
    "Type"
)
VALUES
    (NULL,  'Administration fee',       'Administration fee',       0,  0),
    (7,     'Delay +7 d.',              'Delay +7 d.',              0,  1),
    (14,    'Delay +14 d.',             'Delay +14 d.',             1,  1),
    (21,    'Delay +21 d.',             'Delay +21 d.',             2,  1),
    (NULL,  'Package Damage level 1',   'Package Damage level 1',   0,  2),
    (NULL,  'Package Damage level 2',   'Package Damage level 2',   1,  2),
    (NULL,  'Package Damage level 3',   'Package Damage level 3',   2,  2),
    (NULL,  'Product Damage level 1',   'Product Damage level 1',   0,  3),
    (NULL,  'Product Damage level 2',   'Product Damage level 2',   1,  3),
    (NULL,  'Registration fee',         'Registration fee',         0,  4);

INSERT INTO temp.FeeConfigurationImports
(
    "CountryId",
    "Order",
    "Type",
    "Value",
    "ValueMinimum",
    "ValueType"
)
VALUES
    (NULL,  0,  0,  0.02,   5,      1),
    (NULL,  0,  1,  0,      NULL,   1),
    (NULL,  1,  1,  0.1,    5,      1),
    (NULL,  2,  1,  0.2,    5,      1),
    (NULL,  0,  2,  0.1,    5,      1),
    (NULL,  1,  2,  0.15,   5,      1),
    (NULL,  2,  2,  0.25,   5,      1),
    (NULL,  0,  3,  0.3,    5,      1),
    (NULL,  1,  3,  1,      NULL,   1),
    (NULL,  0,  4,  3,      NULL,   0);
