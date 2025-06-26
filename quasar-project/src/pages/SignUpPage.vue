<template>
  <q-page class="signup-page flex flex-center">
    <q-card class="signup-card q-pa-xl shadow-4">
      <div class="text-h5 text-center text-weight-bold q-mb-lg">Create Your Account</div>
      <q-form @submit="handleSignUp" class="q-gutter-md">
        <q-input v-model="form.firstName" label="First Name" outlined :rules="[val => !!val || 'First name is required']" />
        <q-input v-model="form.lastName" label="Last Name" outlined :rules="[val => !!val || 'Last name is required']" />
        <q-input v-model="form.email" label="Email" type="email" outlined :rules="[val => !!val || 'Email is required', val => isValidEmail(val) || 'Invalid email']" />
        <q-input v-model="form.password" label="Password" type="password" outlined :rules="[val => !!val || 'Password is required', val => val.length >= 6 || 'Min 6 characters']" />
        <q-input v-model="form.confirmPassword" label="Confirm Password" type="password" outlined :rules="[val => val === form.password || 'Passwords do not match']" />
        <q-btn type="submit" color="primary" label="Sign Up" class="full-width q-mt-md" :loading="loading" />
      </q-form>
      <q-banner v-if="error" class="bg-negative text-white q-mt-md">
        <q-icon name="error" /> {{ error }}
      </q-banner>
      <q-banner v-if="success" class="bg-positive text-white q-mt-md">
        <q-icon name="check_circle" /> Registration successful! Redirecting...
      </q-banner>
      <div class="text-center q-mt-md">
        <router-link to="/login" class="text-primary">Already have an account? Log in</router-link>
      </div>
    </q-card>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '../stores/auth'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()
const form = ref({
  firstName: '',
  lastName: '',
  email: '',
  password: '',
  confirmPassword: ''
})
const loading = ref(false)
const success = ref(false)
const error = ref('')

const isValidEmail = (email) => {
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return emailPattern.test(email)
}

const handleSignUp = async () => {
  loading.value = true
  success.value = false
  error.value = ''
  const { firstName, lastName, email, password } = form.value
  const result = await authStore.register({ firstName, lastName, email, password })
  loading.value = false
  if (result.success) {
    success.value = true
    setTimeout(() => router.push('/add-member'), 1200)
  } else {
    error.value = result.message || 'Registration failed. Please try again.'
  }
}
</script>

<style scoped>
.signup-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #e3ecfc 0%, #b6c8f9 100%);
}
.signup-card {
  min-width: 350px;
  max-width: 400px;
  border-radius: 18px;
  background: #fff;
}
</style> 