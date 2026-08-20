import { describe, expect, it } from 'vitest';
import { MemoryRouter } from 'react-router-dom';
import { render, screen } from '@testing-library/react';
import { AddPage } from '../pages/AddPage';

describe('AddPage', () => { it('renders required fields', () => { render(<MemoryRouter><AddPage onSuccess={() => undefined}/></MemoryRouter>); expect(screen.getByLabelText('Name')).toBeTruthy(); expect(screen.getByLabelText('Pincode')).toBeTruthy(); }); });
