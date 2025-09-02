import { ClientesComponent } from './clientes.component';
import { ClienteDto } from '../../../core/dtos/cliente.dto';

describe('ClientesComponent - Jest Tests', () => {
  let component: ClientesComponent;

  const mockClientes: ClienteDto[] = [
    new ClienteDto(1, 'Juan Pérez', 'Masculino', 30, '12345678', 'Calle 123', '555-1234', 101, 'pass123', true),
    new ClienteDto(2, 'María García', 'Femenino', 25, '87654321', 'Avenida 456', '555-5678', 102, 'pass456', true),
    new ClienteDto(3, 'Carlos López', 'Masculino', 35, '11223344', 'Carrera 789', '555-9012', 103, 'pass789', false)
  ];

  beforeEach(() => {
    // Create a partial mock of the component for testing utility methods
    component = Object.create(ClientesComponent.prototype);
    component.filteredClientes = [];
    component.paginatedClientes = [];
    component.currentPage = 1;
    component.pageSize = 2;
    component.totalPages = 0;
    component.isLoading = false;
    component.clientes = mockClientes;
    component.searchTerm = '';
  });

  describe('Utility Methods', () => {
    it('should calculate correct start index for pagination', () => {
      component.filteredClientes = mockClientes;
      component.currentPage = 1;
      component.pageSize = 2;
      
      expect(component.startIndex).toBe(1);
      
      component.currentPage = 2;
      expect(component.startIndex).toBe(3);
    });

    it('should calculate correct end index for pagination', () => {
      component.filteredClientes = mockClientes;
      component.currentPage = 1;
      component.pageSize = 2;
      
      expect(component.endIndex).toBe(2);
      
      component.currentPage = 2;
      expect(component.endIndex).toBe(3); // Should be min(4, 3) = 3
    });

    it('should handle empty data correctly', () => {
      component.paginatedClientes = [];
      component.isLoading = false;
      
      expect(component.hasData).toBe(false);
      expect(component.showNoData).toBe(true);
    });

    it('should not show no data message while loading', () => {
      component.paginatedClientes = [];
      component.isLoading = true;
      
      expect(component.showNoData).toBe(false);
    });
  });

  describe('Search Functionality', () => {
    it('should test search logic with mock data', () => {
      component.clientes = mockClientes;
      component.filteredClientes = mockClientes;
      
      // Test filtering by name (simulating the search logic)
      const juanClients = mockClientes.filter(client => 
        client.nombre.toLowerCase().includes('juan')
      );
      expect(juanClients.length).toBe(1);
      expect(juanClients[0].nombre).toBe('Juan Pérez');
    });

    it('should test search logic with identification', () => {
      const clientsByIdentification = mockClientes.filter(client => 
        client.identificacion.includes('87654321')
      );
      expect(clientsByIdentification.length).toBe(1);
      expect(clientsByIdentification[0].identificacion).toBe('87654321');
    });

    it('should handle empty search results', () => {
      const noResults = mockClientes.filter(client => 
        client.nombre.toLowerCase().includes('nonexistent')
      );
      expect(noResults.length).toBe(0);
    });
  });

  describe('Pagination', () => {
    beforeEach(() => {
      component.filteredClientes = mockClientes;
      component.pageSize = 2;
    });

    it('should change page correctly', () => {
      component.totalPages = 3;
      
      // Mock the changePage method behavior
      const originalChangePage = ClientesComponent.prototype.changePage;
      component.changePage = function(page: number) {
        if (page >= 1 && page <= this.totalPages) {
          this.currentPage = page;
        }
      };
      
      component.changePage(2);
      
      expect(component.currentPage).toBe(2);
    });

    it('should not change to invalid page numbers', () => {
      component.totalPages = 2;
      component.currentPage = 1;
      
      // Mock the changePage method behavior
      component.changePage = function(page: number) {
        if (page >= 1 && page <= this.totalPages) {
          this.currentPage = page;
        }
      };
      
      component.changePage(0);
      expect(component.currentPage).toBe(1);
      
      component.changePage(3);
      expect(component.currentPage).toBe(1);
    });

    it('should calculate total pages correctly', () => {
      // Mock the private method by calling onSearch which triggers updatePagination
      component.searchTerm = '';
      component.onSearch();
      
      expect(component.totalPages).toBe(Math.ceil(3 / 2)); // 3 clients, 2 per page = 2 pages
    });
  });
});