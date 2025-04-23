-- V1__Create_Table_Products.sql
CREATE TABLE IF NOT EXISTS products (
    id BIGSERIAL PRIMARY KEY,
    tipo INTEGER NOT NULL,
    peso_kg DECIMAL NOT NULL,
    preco_quilo DECIMAL NOT NULL,
    imagem_url TEXT NOT NULL,
    avaliavel BOOLEAN DEFAULT FALSE,
    id_pescador BIGINT NOT NULL
);

-- V2__Add_FK_Products_IdPescador.sql
ALTER TABLE products
ADD CONSTRAINT fk_users FOREIGN KEY(id_pescador) REFERENCES users(id);