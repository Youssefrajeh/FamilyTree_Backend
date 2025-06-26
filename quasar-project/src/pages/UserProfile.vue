<template>
  <q-page class="profile-page flex flex-center">
    <q-card class="profile-card q-pa-xl shadow-4">
      <div class="flex flex-center q-mb-lg">
        <q-avatar size="80px">
          <img v-if="user && user.avatarUrl" :src="user.avatarUrl" alt="User" />
          <q-icon v-else name="account_circle" size="80px" color="primary" />
        </q-avatar>
      </div>
      <div class="text-h5 text-center text-weight-bold q-mb-xs">{{ user?.firstName || 'User' }} {{ user?.lastName || '' }}</div>
      <div class="text-body1 text-center text-grey-8 q-mb-md">{{ user?.email }}</div>
      <q-btn color="primary" icon="edit" label="Edit Profile" @click="editProfile = true" class="full-width q-mt-md" />
      <q-dialog v-model="editProfile">
        <q-card class="q-pa-lg">
          <q-card-section>
            <div class="text-h6 q-mb-md">Edit Profile</div>
            <q-input v-model="editForm.firstName" label="First Name" outlined class="q-mb-md" />
            <q-input v-model="editForm.lastName" label="Last Name" outlined class="q-mb-md" />
            <q-input v-model="editForm.email" label="Email" outlined class="q-mb-md" />
          </q-card-section>
          <q-card-actions align="right">
            <q-btn flat label="Cancel" color="primary" v-close-popup />
            <q-btn color="primary" label="Save" @click="saveProfile" />
          </q-card-actions>
        </q-card>
      </q-dialog>
    </q-card>
  </q-page>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore()
const user = computed(() => authStore.user)

const editProfile = ref(false)
const editForm = ref({
  firstName: user.value?.firstName || '',
  lastName: user.value?.lastName || '',
  email: user.value?.email || ''
})

const saveProfile = () => {
  // Here you would call an API or update the store
  authStore.user = { ...authStore.user, ...editForm.value }
  editProfile.value = false
}
</script>

<style scoped>
.profile-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #e3ecfc 0%, #b6c8f9 100%);
}
.profile-card {
  min-width: 350px;
  max-width: 400px;
  border-radius: 18px;
  background: #fff;
}
</style> 