SET IDENTITY_INSERT Customers ON
GO

INSERT INTO Products (Id, Name, Price) VALUES
 (100, 'Ergonomic Metal Hat', 95),
 (101, 'Delicate Frozen Tuna', 1),
 (102, 'Handcrafted Quantum Computer', 86),
 (103, 'Intelligent Cotton Flange', 5),
 (104, 'Unbranded Frozen Chicken', 29),
 (105, 'Handmade Soft Scarf', 74),
 (106, 'Refined Tubular Flashing', 94),
 (107, 'Generic Rubber Socks', 44),
 (108, 'Fantastic Tall Table', 14),
 (109, 'Small Spicy Sausages', 89);

INSERT INTO Customers (Id, Name, City, State) VALUES (10, 'Jaunita Mohr', 'Davinfurt', 'NM');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (11, 10, '2022-04-27');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (12, 11, 105, 3, 44),
 (13, 11, 104, 3, 14),
 (14, 11, 105, 6, 1);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (15, 10, '2022-08-02');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (16, 15, 103, 8, 14),
 (17, 15, 106, 10, 89),
 (18, 15, 103, 7, 74),
 (19, 15, 109, 4, 94);

INSERT INTO Customers (Id, Name, City, State) VALUES (11, 'Hailie Kuhn', 'New Braulio', 'OH');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (21, 11, '2022-09-18');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (22, 21, 103, 6, 95),
 (23, 21, 102, 8, 95),
 (24, 21, 107, 9, 94);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (25, 11, '2022-10-10');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (26, 25, 104, 8, 95),
 (27, 25, 100, 8, 5);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (28, 11, '2023-01-10');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (29, 28, 100, 6, 86),
 (30, 28, 100, 8, 29);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (31, 11, '2022-11-16');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (32, 31, 105, 10, 86),
 (33, 31, 104, 10, 5);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (34, 11, '2022-06-13');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (35, 34, 109, 7, 86),
 (36, 34, 106, 10, 94),
 (37, 34, 101, 5, 5);

INSERT INTO Customers (Id, Name, City, State) VALUES (12, 'Braden Rogahn', 'Bednarhaven', 'IA');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (39, 12, '2023-04-01');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (40, 39, 109, 10, 14),
 (41, 39, 101, 7, 86),
 (42, 39, 103, 3, 86),
 (43, 39, 106, 1, 94),
 (44, 39, 103, 5, 44);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (45, 12, '2022-08-19');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (46, 45, 105, 6, 14),
 (47, 45, 105, 5, 1);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (48, 12, '2022-06-07');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (49, 48, 102, 7, 74),
 (50, 48, 105, 7, 1),
 (51, 48, 104, 6, 5),
 (52, 48, 109, 9, 95);

INSERT INTO Customers (Id, Name, City, State) VALUES (13, 'Denis Streich', 'Graycetown', 'AZ');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (54, 13, '2022-09-13');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (55, 54, 101, 8, 5),
 (56, 54, 107, 5, 44),
 (57, 54, 106, 3, 5),
 (58, 54, 100, 9, 89);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (59, 13, '2022-06-11');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (60, 59, 101, 1, 14),
 (61, 59, 106, 10, 95),
 (62, 59, 106, 1, 89);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (63, 13, '2022-11-17');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (64, 63, 103, 7, 1),
 (65, 63, 106, 7, 95),
 (66, 63, 108, 8, 89),
 (67, 63, 103, 4, 1),
 (68, 63, 108, 2, 14);

INSERT INTO Customers (Id, Name, City, State) VALUES (14, 'Justice Pfannerstill', 'New Crystel', 'PA');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (70, 14, '2022-09-09');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (71, 70, 107, 6, 86),
 (72, 70, 106, 2, 74),
 (73, 70, 107, 7, 89);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (74, 14, '2023-02-10');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (75, 74, 103, 1, 1);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (76, 14, '2022-10-23');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (77, 76, 100, 10, 86),
 (78, 76, 107, 8, 86),
 (79, 76, 100, 10, 94);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (80, 14, '2022-07-26');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (81, 80, 105, 10, 89),
 (82, 80, 108, 8, 1),
 (83, 80, 103, 8, 94),
 (84, 80, 108, 3, 94),
 (85, 80, 105, 5, 44);

INSERT INTO Customers (Id, Name, City, State) VALUES (15, 'Maverick Schroeder', 'West Tyreltown', 'MA');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (87, 15, '2022-11-19');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (88, 87, 102, 7, 14);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (89, 15, '2023-01-31');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (90, 89, 106, 5, 94),
 (91, 89, 101, 5, 44);

INSERT INTO Customers (Id, Name, City, State) VALUES (16, 'Garry Hermiston', 'North Donna', 'MS');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (93, 16, '2022-12-16');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (94, 93, 104, 4, 1);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (95, 16, '2023-02-21');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (96, 95, 104, 10, 89);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (97, 16, '2022-07-03');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (98, 97, 108, 6, 86),
 (99, 97, 104, 5, 44),
 (100, 97, 101, 6, 44),
 (101, 97, 100, 2, 86);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (102, 16, '2022-05-01');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (103, 102, 101, 3, 44),
 (104, 102, 104, 6, 14);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (105, 16, '2022-09-26');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (106, 105, 104, 5, 86),
 (107, 105, 101, 1, 86),
 (108, 105, 109, 10, 29);

INSERT INTO Customers (Id, Name, City, State) VALUES (17, 'Barrett Ziemann', 'East Kaleyberg', 'ND');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (110, 17, '2023-02-12');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (111, 110, 108, 3, 1),
 (112, 110, 107, 1, 95),
 (113, 110, 100, 8, 89),
 (114, 110, 107, 1, 44),
 (115, 110, 102, 3, 94);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (116, 17, '2022-09-03');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (117, 116, 104, 8, 44);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (118, 17, '2022-05-08');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (119, 118, 101, 7, 89),
 (120, 118, 109, 1, 74),
 (121, 118, 108, 10, 5),
 (122, 118, 103, 5, 44);

INSERT INTO Customers (Id, Name, City, State) VALUES (18, 'Laurence Batz', 'West Jaydaville', 'NC');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (124, 18, '2022-06-05');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (125, 124, 101, 10, 44),
 (126, 124, 103, 6, 95);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (127, 18, '2023-03-07');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (128, 127, 107, 1, 89),
 (129, 127, 109, 7, 29),
 (130, 127, 107, 4, 29),
 (131, 127, 102, 8, 29);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (132, 18, '2022-08-29');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (133, 132, 101, 9, 44);

INSERT INTO Customers (Id, Name, City, State) VALUES (19, 'Alessandra Marvin', 'East Carolynfort', 'WA');

INSERT INTO Orders (Id, CustomerId, Date) VALUES (135, 19, '2022-12-29');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (136, 135, 100, 7, 95),
 (137, 135, 100, 5, 5),
 (138, 135, 108, 1, 94),
 (139, 135, 104, 9, 89),
 (140, 135, 105, 3, 95);

INSERT INTO Orders (Id, CustomerId, Date) VALUES (141, 19, '2022-07-22');

INSERT INTO Lines (Id, OrderId, ProductId, Quantity, PriceEach) VALUES
 (142, 141, 100, 4, 5),
 (143, 141, 100, 9, 5),
 (144, 141, 104, 1, 29),
 (145, 141, 109, 5, 89);

GO
SET IDENTITY_INSERT Customers OFF