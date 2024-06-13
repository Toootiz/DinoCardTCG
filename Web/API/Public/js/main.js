/**
 * @param {number} alpha Indicated the transparency of the color
 * @returns {string} A string of the form 'rgba(240, 50, 123, 1.0)' that represents a color
 */
function random_color(alpha = 1.0) {
    const r_c = () => Math.round(Math.random() * 255);
    return `rgba(${r_c()}, ${r_c()}, ${r_c()}, ${alpha})`;
  }
  
  Chart.defaults.font.size = 16;
  
  const fetchData = async (url) => {
    try {
      const response = await fetch(`http://localhost:3000${url}`);
      if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
      return await response.json();
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };
  
  const createChart = (ctx, type, data, options) => {
    new Chart(ctx, {
      type: type,
      data: data,
      options: options,
    });
  };
  
  document.addEventListener("DOMContentLoaded", async () => {
    const cardsData = await fetchData("/api/cards");
    const playersData = await fetchData("/api/players");
    const decksData = await fetchData("/api/decks");
    const matchesData = await fetchData("/api/matches");
  
    // console.log(cardsData, playersData, decksData, matchesData); // Verificar datos
  
    if (cardsData && playersData && decksData && matchesData) {
      // Cartas y Habilidades
      const cardLabels = cardsData.cards.map((card) => card.Nombre);
      const healthPoints = cardsData.cards.map((card) => card.Puntos_de_Vida);
      const attackPoints = cardsData.cards.map((card) => card.Puntos_de_ataque);
  
      const ctxCards = document.getElementById("cardsChart").getContext("2d");
      createChart(
        ctxCards,
        "line",
        {
          labels: cardLabels,
          datasets: [
            {
              label: "Puntos de Vida",
              data: healthPoints,
              backgroundColor: "rgba(75, 192, 192, 0.2)",
              borderColor: "rgba(75, 192, 192, 1)",
              borderWidth: 1,
            },
            {
              label: "Puntos de Ataque",
              data: attackPoints,
              backgroundColor: "rgba(153, 102, 255, 0.2)",
              borderColor: "rgba(153, 102, 255, 1)",
              borderWidth: 1,
            },
          ],
        },
        {
          scales: {
            y: {
              beginAtZero: true,
            },
          },
        }
      );
  
      // Estadísticas de Jugadores
      const playerLabels = playersData.players.map((player) => player.nombre_jugador);
      const wins = playersData.players.map((player) => player.partidas_ganadas);
      const losses = playersData.players.map((player) => player.partidas_perdidas);
      //console.log('fijrjgri',playersData);
      //console.log('asdasdasdasdads',playerLabels, wins, losses);
  
      const ctxPlayers = document.getElementById("playersChart").getContext("2d");
      createChart(
        ctxPlayers,
        "bar",
        {
          labels: playerLabels,
          datasets: [
            {
              label: "Partidas Ganadas",
              data: wins,
              backgroundColor: "rgba(75, 192, 192, 0.6)",
              borderColor: "rgba(75, 192, 192, 1)",
              borderWidth: 1,
            },
            {
              label: "Partidas Perdidas",
              data: losses,
              backgroundColor: "rgba(255, 99, 132, 0.6)",
              borderColor: "rgba(255, 99, 132, 1)",
              borderWidth: 1,
            },
          ],
        },
        {
          scales: {
            y: {
              beginAtZero: true,
            },
          },
        }
      );
  
     // Decks y Cartas
const deckLabels = decksData.decks.map((deck) => deck.nombre);
const cardCounts = decksData.decks.map((deck) => deck.cantidad_apariciones);

// Función para generar colores aleatorios
function getRandomColor() {
  const r = Math.floor(Math.random() * 255);
  const g = Math.floor(Math.random() * 255);
  const b = Math.floor(Math.random() * 255);
  return `rgba(${r}, ${g}, ${b}, 0.2)`;
}

// Generar colores aleatorios para cada sección de la gráfica
const backgroundColors = deckLabels.map(() => getRandomColor());
const borderColors = backgroundColors.map(color => color.replace('0.2', '1'));

const ctxDecks = document.getElementById("decksChart").getContext("2d");
createChart(
  ctxDecks,
  "pie",
  {
    labels: deckLabels,
    datasets: [
      {
        label: "Cartas más Utilizadas",
        data: cardCounts,
        backgroundColor: backgroundColors,
        borderColor: borderColors,
        borderWidth: 1,
      },
    ],
  },
  {
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  }
);

    }
  });
  