import { afterEach, describe, expect, it } from 'vitest';
import { MemoryRouter } from 'react-router-dom';
import { cleanup, fireEvent, render, screen } from '@testing-library/react';
import { AddPage } from '../pages/AddPage';

afterEach(cleanup);

describe('AddPage', () => { it('renders required fields', () => { render(<MemoryRouter><AddPage onSuccess={() => undefined}/></MemoryRouter>); expect(screen.getByLabelText('Name')).toBeTruthy(); expect(screen.getByLabelText('Pincode')).toBeTruthy(); }); });

it('requires an age', () => { render(<MemoryRouter><AddPage onSuccess={() => undefined}/></MemoryRouter>); fireEvent.click(screen.getByRole('button', { name: 'Add user' })); expect(screen.getByText('Age must be an integer from 0–120.')).toBeTruthy(); });
