function inicializarGraficoRosca(pendentes, pagas, atrasadas) {
    var elemento = document.getElementById('graficoRosca');

    if (!elemento)
        return;

    var contexto = elemento.getContext('2d');

    new Chart(contexto, {
        type: 'doughnut',
        data: {
            labels: ['Pendentes', 'Pagas', 'Atrasadas'],
            datasets: [{
                data: [pendentes, pagas, atrasadas],
                backgroundColor: [
                    'rgba(255, 193, 7, 0.8)',
                    'rgba(40, 167, 69, 0.8)',
                    'rgba(220, 53, 69, 0.8)'
                ],
                borderColor: 'rgba(255, 255, 255, 0.05)',
                borderWidth: 2,
                hoverOffset: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '65%',
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        color: 'rgba(255, 255, 255, 0.7)',
                        font: { size: 12, family: 'Inter' },
                        padding: 16,
                        usePointStyle: true,
                        pointStyleWidth: 10
                    }
                }
            }
        }
    });
}

function inicializarGraficoLinha(rotulos, valores) {
    var elemento = document.getElementById('graficoLinha');

    if (!elemento)
        return;

    var contexto = elemento.getContext('2d');

    var gradiente = contexto.createLinearGradient(0, 0, 0, 250);
    gradiente.addColorStop(0, 'rgba(40, 167, 69, 0.25)');
    gradiente.addColorStop(1, 'rgba(40, 167, 69, 0)');

    new Chart(contexto, {
        type: 'line',
        data: {
            labels: rotulos,
            datasets: [{
                label: 'Pagamentos Recebidos (R$)',
                data: valores,
                borderColor: '#28a745',
                backgroundColor: gradiente,
                fill: true,
                tension: 0.4,
                pointBackgroundColor: '#28a745',
                pointBorderColor: 'rgba(40, 167, 69, 0.3)',
                pointBorderWidth: 3,
                pointRadius: 5,
                pointHoverRadius: 8,
                borderWidth: 2.5
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    ticks: {
                        color: 'rgba(255, 255, 255, 0.5)',
                        font: { size: 11, family: 'Inter' }
                    },
                    grid: { color: 'rgba(255, 255, 255, 0.04)' }
                },
                y: {
                    ticks: {
                        color: 'rgba(255, 255, 255, 0.5)',
                        font: { size: 11, family: 'Inter' }
                    },
                    grid: { color: 'rgba(255, 255, 255, 0.04)' },
                    beginAtZero: true
                }
            },
            plugins: {
                legend: {
                    labels: {
                        color: 'rgba(255, 255, 255, 0.7)',
                        font: { size: 12, family: 'Inter' },
                        usePointStyle: true
                    }
                }
            }
        }
    });
}
