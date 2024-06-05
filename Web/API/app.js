"use strict";

import express from "express";
import mysql from "mysql2/promise";
import bodyParser from 'body-parser';
import cors from 'cors'; // Importar el paquete cors

const app = express();
const port = 3000;

app.use(cors()); // Habilitar CORS
app.use(express.json());
app.use(bodyParser.urlencoded({ extended: true }));

async function connectToDB() {
    return mysql.createConnection({
        host: "localhost",
        user: "p1",
        password: "123456",
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
        const [results, fields] = await connection.execute("SELECT * FROM carta_habilidad_detalle ORDER BY id_carta;");
        const c = { "cards": results };
        res.status(200).json(c);
    } catch (error) {
        console.error("Error fetching cards:", error);
        res.status(500).json({ error: error.message });
    } finally {
        if (connection) {
            connection.end();
        }
    }
});


// Fetch decks for a specific player
app.get("/api/deckss/:id_jugador", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute("SELECT * FROM vista_decks_cartas WHERE id_jugador = ?", [req.params.id_jugador]);
        const decks = { "decks": results };
        res.status(200).json(decks);
    } catch (error) {
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});


app.post("/api/guardardeck", async (req, res) => {
    const { id_jugador, nombre_deck, descripcion_deck, id_carta1, id_carta2, id_carta3, id_carta4, id_carta5, id_carta6, id_carta7, id_carta8, id_carta9, id_carta10 } = req.body;

    let connection = null;
    try {
        connection = await connectToDB();
        const query = `
            INSERT INTO deck (
                id_jugador, nombre_deck, descripcion_deck, id_carta1, id_carta2, id_carta3, id_carta4, id_carta5, id_carta6, id_carta7, id_carta8, id_carta9, id_carta10
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        `;
        const values = [id_jugador, nombre_deck, descripcion_deck, id_carta1, id_carta2, id_carta3, id_carta4, id_carta5, id_carta6, id_carta7, id_carta8, id_carta9, id_carta10];
        const [results] = await connection.execute(query, values);

        res.status(201).json({ message: "Deck created successfully", deckId: results.insertId });
    } catch (error) {
        console.error("Error fetching deck1:", error);
        res.status(500).json(error);
    } finally {
        if (connection) {
            await connection.end();  // Asegúrate de usar await aquí
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
        console.error("Error fetching card by ID:", error);
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

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
        console.error("Error adding card:", error);
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

app.post('/register', async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const { nombre, contrasena } = req.body;
        const [results] = await connection.execute('INSERT INTO jugador (nombre, contrasena) VALUES (?, ?)', [nombre, contrasena]);
        res.send('Usuario registrado exitosamente');
    } catch (error) {
        console.error("Error registering user:", error);
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

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
        console.error("Error logging in user:", error);
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

app.get("/api/players", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute("SELECT * FROM vista_estadisticas_jugadores;");
        const c = { "players": results };
        res.status(200).json(c);
    } catch (error) {
        console.error("Error fetching players:", error);
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

app.get("/api/decks", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute("SELECT * FROM vista_decks_cartas;");
        const c = { "decks": results };
        res.status(200).json(c);
    } catch (error) {
        console.error("Error fetching decks:", error);
        res.status(500).json(error);
    } finally {
        if (connection) {
            connection.end();
        }
    }
});

app.get("/api/matches", async (req, res) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute("SELECT * FROM vista_resultados_partidas;");
        const c = { "matches": results };
        res.status(200).json(c);
    } catch (error) {
        console.error("Error fetching matches:", error);
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