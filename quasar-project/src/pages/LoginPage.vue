<template>
  <q-page class="flex flex-center bg-gradient-to-br from-blue-50 to-indigo-100">
    <div class="q-pa-md" style="max-width: 400px; width: 100%;">
      <q-card class="q-pa-lg shadow-lg">
        <q-card-section class="text-center">
          <h4 class="text-h4 q-mb-md text-primary">Family Tree</h4>
          <p class="text-subtitle1 text-grey-7">Welcome back! Please sign in to continue.</p>
        </q-card-section>

        <q-card-section>
          <q-form @submit="handleLogin" class="q-gutter-md">
            <q-input
              v-model="form.email"
              type="email"
              label="Email Address"
              outlined
              :rules="[val => !!val || 'Email is required', val => isValidEmail(val) || 'Please enter a valid email']"
              dense
            >
              <template v-slot:prepend>
                <q-icon name="email" />
              </template>
            </q-input>

            <q-input
              v-model="form.password"
              :type="showPassword ? 'text' : 'password'"
              label="Password"
              outlined
              :rules="[val => !!val || 'Password is required']"
              dense
            >
              <template v-slot:prepend>
                <q-icon name="lock" />
              </template>
              <template v-slot:append>
                <q-icon
                  :name="showPassword ? 'visibility_off' : 'visibility'"
                  class="cursor-pointer"
                  @click="showPassword = !showPassword"
                />
              </template>
            </q-input>

            <q-btn
              type="submit"
              label="Sign In"
              color="primary"
              class="full-width q-mt-md"
              :loading="loading"
              :disable="!form.email || !form.password"
              size="md"
            />
          </q-form>
        </q-card-section>

        <q-card-section class="text-center">
          <p class="text-body2">
            Don't have an account?
            <router-link to="/register" class="text-primary text-weight-bold">
              Sign up here
            </router-link>
          </p>
        </q-card-section>

        <q-banner v-if="error" class="bg-negative text-white q-mt-md">
          <template v-slot:avatar>
            <q-icon name="error" />
          </template>
          {{ error }}
        </q-banner>
      </q-card>
    </div>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const $q = useQuasar()
const authStore = useAuthStore()

const form = ref({
  email: '',
  password: ''
})

const showPassword = ref(false)
const loading = ref(false)
const error = ref('')

const isValidEmail = (email) => {
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return emailPattern.test(email)
}

const handleLogin = async () => {
  loading.value = true
  error.value = ''

  try {
    const result = await authStore.login(form.value)
    
    if (result.success) {
      $q.notify({
        type: 'positive',
        message: 'Welcome back! Login successful.',
        position: 'top'
      })
      router.push('/dashboard')
    } else {
      error.value = result.message
    }
  } catch (err) {
    console.error('Login error:', err)
    error.value = 'An unexpected error occurred. Please try again.'
  } finally {
    loading.value = false
  }
}
</script>

<style lang="scss" scoped>
.q-page {
  min-height: 100vh;
}

.q-card {
  border-radius: 12px;
}

.text-primary {
  color: #1976d2 !important;
}

.cursor-pointer {
  cursor: pointer;
}
</style> 