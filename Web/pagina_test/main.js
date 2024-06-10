/**
 * @param {number} alpha Indicated the transparency of the color
 * @returns {string} A string of the form 'rgba(240, 50, 123, 1.0)' that represents a color
 */
function random_color(alpha=1.0) {
    const r_c = () => Math.round(Math.random() * 255)
    return `rgba(${r_c()}, ${r_c()}, ${r_c()}, ${alpha})`
}

Chart.defaults.font.size = 16;

const fetchData = async (url) => {
    try {
        const response = await fetch(`http://localhost:3000${url}`);
        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        return await response.json();
    } catch (error) {
        console.error('Error fetching data:', error);
    }
};

const createChart = (ctx, type, data, options) => {
    new Chart(ctx, {
        type: type,
        data: data,
        options: options,
    });
};

document.addEventListener('DOMContentLoaded', async () => {
    const cardsData = await fetchData('/api/cards');
    const playersData = await fetchData('/api/players');
    const decksData = await fetchData('/api/decks');
    const matchesData = await fetchData('/api/matches');

    if (cardsData && playersData && decksData && matchesData) {
        // Cartas y Habilidades
        const cardLabels = cardsData.cards.map(card => card.Nombre);
        const healthPoints = cardsData.cards.map(card => card.Puntos_de_Vida);
        const attackPoints = cardsData.cards.map(card => card.Puntos_de_ataque);

        const ctxCards = document.getElementById('cardsChart').getContext('2d');
        createChart(ctxCards, 'line', {
            labels: cardLabels,
            datasets: [
                {
                    label: 'Puntos de Vida',
                    data: healthPoints,
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 1,
                },
                {
                    label: 'Puntos de Ataque',
                    data: attackPoints,
                    backgroundColor: 'rgba(153, 102, 255, 0.2)',
                    borderColor: 'rgba(153, 102, 255, 1)',
                    borderWidth: 1,
                },
            ],
        }, {
            scales: {
                y: {
                    beginAtZero: true,
                },
            },
        });

        // Estadísticas de Jugadores
        const playerLabels = playersData.players.map(player => player.nombre);
        const wins = playersData.players.map(player => player.partidas_ganadas);
        const losses = playersData.players.map(player => player.partidas_perdidas);

        const ctxPlayers = document.getElementById('playersChart').getContext('2d');
        createChart(ctxPlayers, 'pie', {
            labels: playerLabels,
            datasets: [
                {
                    label: 'Partidas Ganadas',
                    data: wins,
                    backgroundColor: wins.map(() => random_color(0.6)),
                    borderColor: 'rgba(0, 0, 0, 1.0)',
                    borderWidth: 1,
                },
                {
                    label: 'Partidas Perdidas',
                    data: losses,
                    backgroundColor: losses.map(() => random_color(0.6)),
                    borderColor: 'rgba(0, 0, 0, 1.0)',
                    borderWidth: 1,
                },
            ],
        });

        // Decks y Cartas
        const deckLabels = decksData.decks.map(deck => deck.nombre_deck);
        const cardCounts = decksData.decks.map(deck => deck.cantidad_cartas);

        const ctxDecks = document.getElementById('decksChart').getContext('2d');
        createChart(ctxDecks, 'bar', {
            labels: deckLabels,
            datasets: [
                {
                    label: 'Cantidad de Cartas',
                    data: cardCounts,
                    backgroundColor: 'rgba(255, 206, 86, 0.2)',
                    borderColor: 'rgba(255, 206, 86, 1)',
                    borderWidth: 1,
                },
            ],
        }, {
            scales: {
                y: {
                    beginAtZero: true,
                },
            },
        });

        // Resultados de Partidas
        const matchLabels = matchesData.matches.map(match => `Partida ${match.id_partida}`);
        const turnCounts = matchesData.matches.map(match => match.cantidad_turnos);

        const ctxMatches = document.getElementById('matchesChart').getContext('2d');
        createChart(ctxMatches, 'line', {
            labels: matchLabels,
            datasets: [
                {
                    label: 'Cantidad de Turnos',
                    data: turnCounts,
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 1,
                },
            ],
        }, {
            scales: {
                y: {
                    beginAtZero: true,
                },
            },
        });
    }
});