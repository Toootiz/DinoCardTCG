"use strict";

import express from "express";
import mysql from "mysql2/promise";

const app = express();
const port = 3000;

app.use(express.json());

async function connectToDB() {
    return mysql.createConnection({
        host: "localhost",
        user: "root",
        password: "root",
        database: "dinocard_db"
    });
}

app.get("/", (req, res) => {
    res.status(200).send("API is running");
});

// Fetch all cards
app.get("/api/cards", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const [results] = await connection.execute("SELECT * FROM carta");
        res.status(200).json(results);
    } catch (error) {
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

// Fetch a single card by ID
app.get("/api/cards/:id", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const [results] = await connection.execute("SELECT * FROM carta WHERE id_carta = ?", [req.params.id]);
        results.length > 0 ? res.status(200).json(results[0]) : res.status(404).send("Card not found");
    } catch (error) {
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

// Insert a new card
app.post("/api/cards", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const { Nombre, Puntos_de_Vida, Puntos_de_ataque, Coste_en_elixir, Habilidad } = req.body;
        const [results] = await connection.execute(
            "INSERT INTO carta (Nombre, Puntos_de_Vida, Puntos_de_ataque, Coste_en_elixir, Habilidad) VALUES (?, ?, ?, ?, ?)",
            [Nombre, Puntos_de_Vida, Puntos_de_ataque, Coste_en_elixir, Habilidad]
        );
        res.status(201).json({ message: "Card added successfully", id_carta: results.insertId });
    } catch (error) {
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

app.listen(port, () => {
    console.log(`Server running on http://localhost:${port}/`);
});
