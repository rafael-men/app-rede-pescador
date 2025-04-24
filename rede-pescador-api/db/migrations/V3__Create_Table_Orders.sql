CREATE TABLE orders (
    id BIGSERIAL PRIMARY KEY,

    buyer_id BIGINT NOT NULL,
    seller_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,
    delivery_method INTEGER NOT NULL, 
    status INTEGER NOT NULL,          

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    -- Foreign Keys
    CONSTRAINT fk_buyer FOREIGN KEY (buyer_id) REFERENCES users(id),
    CONSTRAINT fk_seller FOREIGN KEY (seller_id) REFERENCES users(id),
    CONSTRAINT fk_product FOREIGN KEY (product_id) REFERENCES products(id)
);
