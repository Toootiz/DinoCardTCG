"use strict";

import express from "express";
import mysql from "mysql2/promise";
import bodyParser from 'body-parser';

const app = express();
const port = 3000;

app.use(express.json());
app.use(bodyParser.urlencoded({extended:true}));

async function connectToDB() {
    return mysql.createConnection({
        host: "localhost",
        user: "root",
        password: "1234567890",
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
<<<<<<< HEAD
        const [results, fields] = await connection.execute("SELECT * FROM carta_habilidad_detalle ORDER BY id_carta;");
        const c={"cards":results};
        res.status(200).json(c);
    } catch (error) {
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

app.get("/", (req, res) => {
    res.status(200).send("API is running");
});

// Fetch all cards
app.get("/api/deck2", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute("SELECT * FROM cartas_deck_2;");
        const c={"cards":results};
        res.status(200).json(c);
    } catch (error) {
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

app.get("/api/deck1", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute("SELECT * FROM cartas_deck_1;");
        const c={"cards":results};
        res.status(200).json(c);
=======
        const [results] = await connection.execute("SELECT * FROM carta");
        res.status(200).json({ cards: results });
>>>>>>> 6c6ca13592ba0c919906aa8ef84946631e92ae08
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
            "INSERT INTO carta (nombre, puntos_de_vida, puntos_de_ataque, coste_en_elixir, habilidad) VALUES (?, ?, ?, ?, ?)",
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

// Register a new user
app.post('/register', async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const { nombre, contrasena } = req.body;
        const [results] = await connection.execute('INSERT INTO jugador (nombre, contrasena) VALUES (?, ?)', [nombre, contrasena]);
        res.send('Usuario registrado exitosamente');
    } catch (error) {
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

// Login a user
app.post('/login', async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const { nombre, contrasena } = req.body;
        console.log(req.body);
        const [results] = await connection.execute('SELECT * FROM jugador WHERE nombre = ? AND contrasena = ?', [nombre, contrasena]);
        
        if (results.length > 0) {
            res.send('Usuario autenticado');
        } else {
            res.send('Usuario no autenticado');
        }
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
