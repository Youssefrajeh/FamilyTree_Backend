<script setup>
import { ref, onMounted } from 'vue'
import { familyMembersAPI } from '../services/api'

const members = ref([])
const loading = ref(true)
const error = ref('')

const fetchMembers = async () => {
  loading.value = true
  error.value = ''
  try {
    const res = await familyMembersAPI.getAll()
    members.value = res.data
  } catch (err) {
    error.value = err.response?.data?.message || 'Failed to fetch members.'
  } finally {
    loading.value = false
  }
}

onMounted(fetchMembers)
</script>

<template>
  <q-page class="flex flex-center members-page">
    <q-card class="q-pa-xl shadow-4" style="min-width: 350px; max-width: 600px; width: 100%">
      <div class="text-h5 text-center text-weight-bold q-mb-md">Members</div>
      <q-spinner v-if="loading" color="primary" size="40px" class="q-my-lg" />
      <q-banner v-if="error" class="bg-negative text-white q-mb-md">
        <q-icon name="error" /> {{ error }}
      </q-banner>
      <div v-if="!loading && members.length === 0" class="text-body1 text-center">No members found.</div>
      <q-list v-if="!loading && members.length > 0">
        <q-item v-for="member in members" :key="member.id" class="q-mb-md">
          <q-item-section>
            <div class="text-subtitle1 text-weight-bold">{{ member.name }}</div>
            <div class="text-body2">Relationship: {{ member.relationship }}</div>
            <div class="text-body2">Birthdate: {{ member.birthdate }}</div>
            <div class="text-body2">Notes: {{ member.notes }}</div>
          </q-item-section>
        </q-item>
      </q-list>
    </q-card>
  </q-page>
</template>

<style scoped>
.members-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #e3ecfc 0%, #b6c8f9 100%);
}
</style> 