import axios from 'axios'

// Create axios instance
export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Request interceptor to add auth token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('familyTree_token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor to handle errors
api.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    if (error.response?.status === 401) {
      // Token expired or invalid
      localStorage.removeItem('familyTree_token')
      localStorage.removeItem('familyTree_user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

// API endpoints
export const authAPI = {
  login: (credentials) => api.post('/auth/login', credentials),
  register: (userData) => api.post('/auth/register', userData),
  verifyToken: () => api.post('/auth/verify-token'),
  changePassword: (passwordData) => api.post('/auth/change-password', passwordData),
  getCurrentUser: () => api.get('/auth/me')
}

export const familyMembersAPI = {
  getAll: () => api.get('/familymembers'),
  getById: (id) => api.get(`/familymembers/${id}`),
  create: (memberData) => api.post('/familymembers', memberData),
  update: (id, memberData) => api.put(`/familymembers/${id}`, memberData),
  delete: (id) => api.delete(`/familymembers/${id}`),
  search: (searchTerm) => api.get(`/familymembers/search?searchTerm=${encodeURIComponent(searchTerm)}`),
  addSpouse: (spouseData) => api.post('/familymembers/spouses', spouseData),
  removeSpouse: (spouseId) => api.delete(`/familymembers/spouses/${spouseId}`)
}

export const familyTreeAPI = {
  getTree: (rootId) => api.get(`/familytree/${rootId}`),
  getRoots: () => api.get('/familytree/roots'),
  getStatistics: () => api.get('/familytree/statistics')
}

export default api 