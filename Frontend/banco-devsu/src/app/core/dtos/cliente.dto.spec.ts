import { ClienteDto } from './cliente.dto';
import { CuentaDto } from './cuenta.dto';

describe('ClienteDto', () => {
  let cliente: ClienteDto;
  const mockCuentas: CuentaDto[] = [];

  beforeEach(() => {
    cliente = new ClienteDto(
      1,
      'Juan Pérez',
      'Masculino',
      30,
      '12345678',
      'Calle 123',
      '555-1234',
      101,
      'password123',
      true,
      mockCuentas
    );
  });

  it('should create a ClienteDto instance', () => {
    expect(cliente).toBeTruthy();
    expect(cliente instanceof ClienteDto).toBe(true);
  });

  it('should have correct properties', () => {
    expect(cliente.id).toBe(1);
    expect(cliente.nombre).toBe('Juan Pérez');
    expect(cliente.genero).toBe('Masculino');
    expect(cliente.edad).toBe(30);
    expect(cliente.identificacion).toBe('12345678');
    expect(cliente.direccion).toBe('Calle 123');
    expect(cliente.telefono).toBe('555-1234');
    expect(cliente.clienteId).toBe(101);
    expect(cliente.contrasena).toBe('password123');
    expect(cliente.estado).toBe(true);
    expect(cliente.cuentas).toEqual(mockCuentas);
  });

  it('should inherit from PersonaDto', () => {
    expect(cliente.nombre).toBeDefined();
    expect(cliente.genero).toBeDefined();
    expect(cliente.edad).toBeDefined();
    expect(cliente.identificacion).toBeDefined();
    expect(cliente.direccion).toBeDefined();
    expect(cliente.telefono).toBeDefined();
  });

  it('should handle optional cuentas parameter', () => {
    const clienteSinCuentas = new ClienteDto(
      2,
      'María García',
      'Femenino',
      25,
      '87654321',
      'Avenida 456',
      '555-5678',
      102,
      'mypassword',
      true
    );

    expect(clienteSinCuentas.cuentas).toBeUndefined();
  });
});