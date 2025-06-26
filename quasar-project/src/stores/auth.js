import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { api } from '../services/api'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null)
  const token = ref(localStorage.getItem('familyTree_token'))
  
  const isAuthenticated = computed(() => !!token.value && !!user.value)
  
  const login = async (credentials) => {
    try {
      const response = await api.post('/auth/login', credentials)
      const { token: newToken, ...userData } = response.data
      
      token.value = newToken
      user.value = userData
      
      localStorage.setItem('familyTree_token', newToken)
      localStorage.setItem('familyTree_user', JSON.stringify(userData))
      
      return { success: true }
    } catch (error) {
      console.error('Login error:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Login failed' 
      }
    }
  }
  
  const register = async (userData) => {
    try {
      const response = await api.post('/auth/register', userData)
      const { token: newToken, ...userInfo } = response.data
      
      token.value = newToken
      user.value = userInfo
      
      localStorage.setItem('familyTree_token', newToken)
      localStorage.setItem('familyTree_user', JSON.stringify(userInfo))
      
      return { success: true }
    } catch (error) {
      console.error('Registration error:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Registration failed' 
      }
    }
  }
  
  const logout = () => {
    token.value = null
    user.value = null
    localStorage.removeItem('familyTree_token')
    localStorage.removeItem('familyTree_user')
  }
  
  const initializeAuth = async () => {
    const storedToken = localStorage.getItem('familyTree_token')
    const storedUser = localStorage.getItem('familyTree_user')
    
    if (storedToken && storedUser) {
      token.value = storedToken
      
      try {
        // Verify token is still valid
        await api.post('/auth/verify-token')
        user.value = JSON.parse(storedUser)
      } catch (err) {
        console.error('Token verification failed:', err)
        // Token is invalid, clear storage
        logout()
      }
    }
  }
  
  return {
    user,
    token,
    isAuthenticated,
    login,
    register,
    logout,
    initializeAuth
  }
}) 