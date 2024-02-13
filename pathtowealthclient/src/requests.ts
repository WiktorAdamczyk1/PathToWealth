export type User = { username: string, email: string }
export type UserFinancialData = {
    initialInvestment: number;
    startInvestmentYear: number;
    startWithdrawalYear: number;
    isInvestmentMonthly: boolean;
    yearlyOrMonthlySavings: number;
    stockAnnualReturn: number;
    stockCostRatio: number;
    bondAnnualReturn: number;
    bondCostRatio: number;
    stockToBondRatio: number;
    retirementDuration: number;
    inflationRate: number;
    withdrawalRate: number;
};
export type RegisterForm = {
    username: string
    email: string
    password: string
    passwordConfirmation: string
}
import router from './router';

export async function login(usernameOrEmail: string, password: string) {
    const request = await fetch('https://localhost:7113/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            usernameOrEmail,
            password
        }),
        credentials: 'include'
    })

    if (request.ok) {
        const responseBody = await request.json();
        const user: User = { username: responseBody.username, email: responseBody.email }
        return user;
    }
    else {
        const errorData = await request.json();
        throw new Error(errorData.detail || 'Login request failed');
    }
}

export async function logout() {
    const request = await fetchWithTokenRefresh('https://localhost:7113/logout', {
        method: 'POST',
        credentials: 'include'
    })

    if (!request.ok) {
        throw new Error('Logout request failed');
    }
}

export async function getUser() {
    const request = await fetchWithTokenRefresh('https://localhost:7113/get-user', {
        method: 'GET',
        credentials: 'include'
    })

    if (request.ok) {
        const responseBody = await request.json();
        const user: User = { username: responseBody.username, email: responseBody.email }
        return user;
    }
    else {
        return null;
    }
}

export async function refreshToken() {
    const request = await fetch('https://localhost:7113/refresh-token', {
        method: 'POST',
        credentials: 'include'
    });

    if (request.ok) {
        const responseBody = await request.json();
        return responseBody.jwtToken;
    } else {
        throw new Error('Token refresh failed');
    }
}

export async function fetchWithTokenRefresh(input: RequestInfo, init?: RequestInit) {
    let response = await fetch(input, init);

    if (response.status === 401) {
        try {
            const refreshResponse = await fetch('https://localhost:7113/refresh-token', {
                method: 'POST',
                credentials: 'include'
            });

            if (refreshResponse.ok) {
                // Retry the fetch with the new token
                response = await fetch(input, init);
            } else {
                throw new Error('Token refresh failed');
            }
        } catch (error) {
            console.error('Token refresh failed:', error);
            throw error;
        }
    }
    return response;
}

export async function register(registerForm: RegisterForm) {
    const response = await fetch('https://localhost:7113/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(registerForm)
    });

    if (response.ok) {
        const data = await response.json();
        return data;
    } else {
        const errorData = await response.json();
        throw new Error(errorData.detail || 'Registration failed');
    }
}

export async function deleteAccount() {
    const request = await fetchWithTokenRefresh('https://localhost:7113/delete-account', {
        method: 'DELETE',
        credentials: 'include'
    });

    if (request.ok) {
        return await request.json();
    } else {
        const error = await request.json();
        throw new Error(error.message || 'Failed to delete account');
    }
}

export async function changePassword(currentPassword: string, newPassword: string) {
    const request = await fetchWithTokenRefresh('https://localhost:7113/change-password', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            currentPassword,
            newPassword
        }),
        credentials: 'include'
    });

    if (request.ok) {
        return await request.json();
    } else {
        const error = await request.json();
        throw new Error(error.message || 'Failed to change password');
    }
}

export async function getUserFinancialData(): Promise<UserFinancialData> {
    const response = await fetchWithTokenRefresh('https://localhost:7113/userfinancialdata', {
        method: 'GET',
        credentials: 'include'
    });

    if (response.ok) {
        const userFinancialData: UserFinancialData = await response.json();
        return userFinancialData;
    } else {
        throw new Error('Failed to get user financial data');
    }
}

export async function updateUserFinancialData(updatedData: UserFinancialData): Promise<void> {
    const response = await fetchWithTokenRefresh('https://localhost:7113/userfinancialdata', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updatedData),
        credentials: 'include'
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Failed to update user financial data');
    }
}

