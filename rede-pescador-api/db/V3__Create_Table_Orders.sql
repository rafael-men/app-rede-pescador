CREATE TABLE IF NOT EXISTS orders (
    id BIGSERIAL PRIMARY KEY,
    buyer_id BIGINT NOT NULL REFERENCES users(id),
    seller_id BIGINT NOT NULL REFERENCES users(id),
    product_id BIGINT NOT NULL REFERENCES products(id),
    delivery_method INTEGER NOT NULL, 
    status INTEGER NOT NULL,           
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
