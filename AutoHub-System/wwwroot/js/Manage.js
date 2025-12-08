// Cars Management JavaScript
document.addEventListener('DOMContentLoaded', function () {
    initializeCarsManagement();
});

function initializeCarsManagement() {
    // Initialize search functionality
    initializeSearch();

    // Initialize filtering
    initializeFiltering();

    // Initialize sorting
    initializeSorting();

    // Initialize export functionality
    initializeExport();

    // Auto-hide success messages
    autoHideAlerts();

    console.log('Cars management initialized successfully');
}

function initializeSearch() {
    const searchInput = document.getElementById('searchInput');
    const table = document.getElementById('carsTable');

    if (searchInput && table) {
        searchInput.addEventListener('input', function () {
            applyAllFilters();
        });
    }
}

function initializeFiltering() {
    const brandFilter = document.getElementById('brandFilter');
    const statusFilter = document.getElementById('statusFilter');
    const priceRangeFilter = document.getElementById('priceRange');
    const clearFiltersBtn = document.getElementById('clearFilters');
    const resetSearchBtn = document.getElementById('resetSearch');

    // Add event listeners to all filters
    if (brandFilter) {
        brandFilter.addEventListener('change', applyAllFilters);
    }

    if (statusFilter) {
        statusFilter.addEventListener('change', applyAllFilters);
    }

    if (priceRangeFilter) {
        priceRangeFilter.addEventListener('change', applyAllFilters);
    }

    // Clear filters button
    if (clearFiltersBtn) {
        clearFiltersBtn.addEventListener('click', clearAllFilters);
    }

    // Reset search button
    if (resetSearchBtn) {
        resetSearchBtn.addEventListener('click', clearAllFilters);
    }
}

function applyAllFilters() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const selectedBrand = document.getElementById('brandFilter').value;
    const selectedStatus = document.getElementById('statusFilter').value;
    const selectedPriceRange = document.getElementById('priceRange').value;

    const table = document.getElementById('carsTable');
    const rows = table.querySelectorAll('tbody tr');
    let visibleCount = 0;

    rows.forEach(row => {
        const brand = row.cells[1].textContent.toLowerCase();
        const model = row.cells[2].textContent.toLowerCase();
        const priceText = row.cells[4].textContent.replace('$', '').replace(',', '');
        const price = parseFloat(priceText);
        const status = row.cells[5].textContent;

        // Search filter
        const matchesSearch = !searchTerm ||
            brand.includes(searchTerm) ||
            model.includes(searchTerm) ||
            priceText.includes(searchTerm);

        // Brand filter
        const matchesBrand = !selectedBrand || brand === selectedBrand.toLowerCase();

        // Status filter
        const matchesStatus = !selectedStatus || status === selectedStatus;

        // Price range filter
        let matchesPrice = true;
        if (selectedPriceRange) {
            const [min, max] = selectedPriceRange.split('-').map(Number);
            matchesPrice = price >= min && price <= max;
        }

        const shouldShow = matchesSearch && matchesBrand && matchesStatus && matchesPrice;
        row.style.display = shouldShow ? '' : 'none';

        if (shouldShow) {
            visibleCount++;
        }
    });

    updateFilteredCount(visibleCount, rows.length);
    toggleNoResultsState(visibleCount);
}

function updateFilteredCount(visible, total) {
    const filteredCountElement = document.getElementById('filteredCount');
    if (filteredCountElement) {
        if (visible === total) {
            filteredCountElement.textContent = `Showing all ${total} cars`;
        } else {
            filteredCountElement.textContent = `Showing ${visible} of ${total} cars`;
        }
    }
}

function toggleNoResultsState(visibleCount) {
    const noResultsState = document.querySelector('.no-results-state');
    const table = document.getElementById('carsTable');

    if (noResultsState && table) {
        if (visibleCount === 0) {
            noResultsState.style.display = 'block';
            table.style.display = 'none';
        } else {
            noResultsState.style.display = 'none';
            table.style.display = 'table';
        }
    }
}

function clearAllFilters() {
    // Clear search input
    document.getElementById('searchInput').value = '';

    // Reset all select filters
    document.getElementById('brandFilter').value = '';
    document.getElementById('statusFilter').value = '';
    document.getElementById('priceRange').value = '';

    // Reapply filters (which will show all rows)
    applyAllFilters();
}

function initializeSorting() {
    const table = document.getElementById('carsTable');
    if (!table) return;

    const headers = table.querySelectorAll('th[data-sort]');

    headers.forEach(header => {
        header.addEventListener('click', function () {
            const sortKey = this.getAttribute('data-sort');
            const isAscending = !this.classList.contains('sort-asc');

            // Remove sort classes from all headers
            headers.forEach(h => {
                h.classList.remove('sort-asc', 'sort-desc');
            });

            // Add sort class to current header
            this.classList.add(isAscending ? 'sort-asc' : 'sort-desc');

            // Sort the table
            sortTable(table, sortKey, isAscending);
        });
    });
}

function sortTable(table, sortKey, ascending) {
    const tbody = table.querySelector('tbody');
    const rows = Array.from(tbody.querySelectorAll('tr:not([style*="display: none"])'));

    rows.sort((a, b) => {
        let aValue, bValue;

        switch (sortKey) {
            case 'id':
                aValue = parseInt(a.cells[0].textContent);
                bValue = parseInt(b.cells[0].textContent);
                break;
            case 'price':
                aValue = parseFloat(a.cells[4].textContent.replace('$', '').replace(',', ''));
                bValue = parseFloat(b.cells[4].textContent.replace('$', '').replace(',', ''));
                break;
            case 'year':
                aValue = parseInt(a.cells[3].textContent);
                bValue = parseInt(b.cells[3].textContent);
                break;
            case 'date':
                aValue = new Date(a.cells[6].textContent);
                bValue = new Date(b.cells[6].textContent);
                break;
            default:
                aValue = a.cells[getColumnIndex(sortKey)].textContent.toLowerCase();
                bValue = b.cells[getColumnIndex(sortKey)].textContent.toLowerCase();
        }

        if (aValue < bValue) return ascending ? -1 : 1;
        if (aValue > bValue) return ascending ? 1 : -1;
        return 0;
    });

    // Clear and re-append sorted rows
    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }

    rows.forEach(row => tbody.appendChild(row));
}

function getColumnIndex(sortKey) {
    const mapping = {
        'brand': 1,
        'model': 2,
        'status': 5
    };
    return mapping[sortKey] || 0;
}

function initializeExport() {
    const exportButton = document.getElementById('exportData');

    if (exportButton) {
        exportButton.addEventListener('click', function (e) {
            e.preventDefault();
            exportToCSV();
        });
    }
}

function exportToCSV() {
    const table = document.getElementById('carsTable');
    if (!table) return;

    const visibleRows = table.querySelectorAll('tbody tr:not([style*="display: none"])');
    let csv = [];

    // Add header row
    const headerRow = [];
    const headers = table.querySelectorAll('thead th');
    headers.forEach(header => {
        if (!header.querySelector('.action-buttons')) {
            headerRow.push(`"${header.textContent}"`);
        }
    });
    csv.push(headerRow.join(','));

    // Add data rows
    visibleRows.forEach(row => {
        const rowData = [];
        const cells = row.querySelectorAll('td');

        cells.forEach(cell => {
            if (!cell.querySelector('.action-buttons')) {
                let text = cell.textContent.trim();
                if (cell.querySelector('.status-badge')) {
                    text = cell.querySelector('.status-badge').textContent;
                }
                if (cell.querySelector('.car-brand')) {
                    text = cell.querySelector('.car-brand').textContent.replace(/[^a-zA-Z0-9\s]/g, '').trim();
                }
                rowData.push(`"${text}"`);
            }
        });

        csv.push(rowData.join(','));
    });

    const csvContent = csv.join('\n');
    downloadCSV(csvContent, 'cars_inventory.csv');
}

function downloadCSV(csvContent, filename) {
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');

    if (link.download !== undefined) {
        const url = URL.createObjectURL(blob);
        link.setAttribute('href', url);
        link.setAttribute('download', filename);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}

function autoHideAlerts() {
    const alerts = document.querySelectorAll('.success-alert, .error-alert');

    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'slideUp 0.3s ease-in';
            setTimeout(() => {
                alert.remove();
            }, 300);
        }, 5000);
    });
}

// Add CSS for slideUp animation
const style = document.createElement('style');
style.textContent = `
    @keyframes slideUp {
        from {
            opacity: 1;
            transform: translateY(0);
        }
        to {
            opacity: 0;
            transform: translateY(-10px);
        }
    }
`;
document.head.appendChild(style);

// Confirmation for delete actions
document.addEventListener('click', function (e) {
    if (e.target.closest('.btn-delete')) {
        e.preventDefault();
        const href = e.target.closest('.btn-delete').href;

        if (confirm('Are you sure you want to delete this car? This action cannot be undone.')) {
            window.location.href = href;
        }
    }
});

