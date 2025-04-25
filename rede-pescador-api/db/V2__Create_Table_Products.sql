CREATE TABLE  products (
    id BIGSERIAL PRIMARY KEY,
    tipo INTEGER NOT NULL,
    nome TEXT NOT NULL,
    descricao TEXT NOT NULL,
    localizacao TEXT NOT NULL,
    peso_kg DECIMAL NOT NULL,
    preco_quilo DECIMAL NOT NULL,
    imagem_url TEXT NOT NULL,
    avaliavel BOOLEAN DEFAULT FALSE,
    id_pescador BIGINT NOT NULL
);

ALTER TABLE products
ADD CONSTRAINT fk_users FOREIGN KEY(id_pescador) REFERENCES users(id);