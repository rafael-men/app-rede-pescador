CREATE TABLE IF NOT EXISTS payment (
    id BIGSERIAL PRIMARY KEY,
    transaction_id VARCHAR(255) NOT NULL,
    amount NUMERIC(18, 2) NOT NULL,
    method VARCHAR(100) NOT NULL,
    status INTEGER NOT NULL, -- Enum será armazenado como int
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    order_id BIGINT NOT NULL,
    CONSTRAINT fk_payment_order FOREIGN KEY (order_id) REFERENCES orders(id)
);
