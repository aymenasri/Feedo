-- Insert sample food products for catalog
-- DisplayLocation: 0 = Default (Catalog), 1 = HomeCarousel (Promotional)

INSERT INTO Products (Name, Description, Price, DiscountPercentage, OldPrice, ImageUrl, IsAvailable, DisplayLocation, CreationA, EstSupprimer, SupperimeA, SupperimerPar)
VALUES 
('Burger Deluxe', 'Juicy gourmet burger with melted cheese, crispy bacon, fresh lettuce, tomatoes, and our special house sauce on a toasted brioche bun. Served with golden fries.', 12.99, NULL, NULL, '/images/products/burger-deluxe.png', 1, 0, GETDATE(), 0, GETDATE(), 0),

('Pizza Margherita', 'Authentic wood-fired pizza with fresh mozzarella, San Marzano tomatoes, fresh basil, and extra virgin olive oil. A classic Italian masterpiece.', 14.99, NULL, NULL, '/images/products/pizza-margherita.png', 1, 0, GETDATE(), 0, GETDATE(), 0),

('Pasta Carbonara', 'Creamy pasta carbonara with crispy pancetta, farm-fresh eggs, aged Parmesan cheese, and freshly cracked black pepper. Pure Italian comfort food.', 13.99, NULL, NULL, '/images/products/pasta-carbonara.png', 1, 0, GETDATE(), 0, GETDATE(), 0),

('Premium Sushi Platter', 'Chef''s selection of premium sushi featuring salmon nigiri, tuna rolls, California rolls, served with wasabi, pickled ginger, and soy sauce.', 24.99, 10, 27.77, '/images/products/sushi-platter.png', 1, 0, GETDATE(), 0, GETDATE(), 0),

('Trio Tacos', 'Three colorful street-style tacos with grilled chicken, fresh pico de gallo, creamy guacamole, cilantro lime crema, and lime wedges.', 11.99, NULL, NULL, '/images/products/tacos-trio.png', 1, 0, GETDATE(), 0, GETDATE(), 0),

('Grilled Salmon Supreme', 'Premium grilled salmon fillet with lemon butter sauce, served with roasted asparagus, herb-roasted potatoes, and fresh herbs.', 22.99, 15, 27.05, '/images/products/salmon-steak.png', 1, 0, GETDATE(), 0, GETDATE(), 0);

-- Note: Products with DisplayLocation = 0 will appear in the Catalog page
-- Products with DiscountPercentage will show discount badges
