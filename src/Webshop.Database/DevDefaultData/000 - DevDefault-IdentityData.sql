-- INSERT Default data for identity development

-- identityusers
INSERT INTO identityusers
VALUES (1, 'TestUser', 'TestUser@test.com', true, 'I36Q4GCXFSX2RIUACTMJXUYS472R4FDG', 'AQAAAAEAACcQAAAAEFDjTALJ0aZ9oLwXGwb6iKwsX4XT3y4foSX//m8LgpATgfR6a0mCcyZ5UXXhecWpbw==', true, NULL, 0);

INSERT INTO identityusers
VALUES (2, 'TestAdmin', 'TestAdmin@test.com', true, 'R6VWCQJV43OIUNZYMMLUJASXCZ2X6Q6G', 'AQAAAAEAACcQAAAAELOA7ZLD3tk6xqlKLUAqX+Dwtma5V8Qr0T7K5k+Yb7v3BBBlmEx6/814tyLodchesA==', true, NULL, 0);

INSERT INTO identityusers
VALUES (3, 'TestSupport', 'TestSupport@test.com', true, 'SOYMRZCXJCUIA6SH3BW6N3BRVCWYFNJB', 'AQAAAAEAACcQAAAAEPEqG4IKemqYBMTtOhUu4r4fR5Vk9URjDlDfqKye3KGTRWEyumLBcgM5Bpmz2QwUjQ==', true, NULL, 0);

-- identityroles
INSERT INTO identityroles
VALUEs (1, 'User');

INSERT INTO identityroles
VALUEs (2, 'Admin');

INSERT INTO identityroles
VALUEs (3, 'Support');

-- identityuserroles
INSERT INTO identityuserroles
VALUES(1, 1);

INSERT INTO identityuserroles
VALUES(2, 2);

INSERT INTO identityuserroles
VALUES(3, 3);
