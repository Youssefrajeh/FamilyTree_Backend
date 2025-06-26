<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated class="navbar-glass">
      <q-toolbar class="q-px-lg">
        <q-btn flat dense round icon="menu" aria-label="Menu" class="q-mr-md" @click="leftDrawerOpen = !leftDrawerOpen" />
        <q-avatar size="36px" class="q-mr-sm">
          <q-icon name="groups" color="primary" size="32px" />
        </q-avatar>
        <span class="text-h6 text-weight-bold text-primary">Family Tree Manager</span>
        <q-space />
        <q-btn flat label="Home" to="/" class="q-mx-sm" />
        <q-btn flat label="Family Tree" to="/tree" class="q-mx-sm" />
        <q-btn flat label="Members" to="/members" class="q-mx-sm" />
        <q-btn flat label="About" to="/about" class="q-mx-sm" />
        <q-btn v-if="!user" color="primary" label="Sign Up" to="/signup" class="q-mx-sm" rounded unelevated />
        <q-btn v-if="user" color="secondary" label="Add Member" to="/add-member" class="q-mx-sm" rounded unelevated />
        <q-btn round flat class="q-ml-md" @click="showProfileMenu = !showProfileMenu">
          <q-avatar size="32px">
            <img v-if="user && user.avatarUrl" :src="user.avatarUrl" alt="User" />
            <q-icon v-else name="account_circle" size="32px" color="primary" />
          </q-avatar>
        </q-btn>
        <q-menu v-model="showProfileMenu" anchor="bottom right" self="top right">
          <q-list style="min-width: 150px;">
            <q-item clickable v-close-popup to="/profile">
              <q-item-section avatar><q-icon name="person" /></q-item-section>
              <q-item-section>Profile</q-item-section>
            </q-item>
            <q-item clickable v-close-popup @click="logout">
              <q-item-section avatar><q-icon name="logout" /></q-item-section>
              <q-item-section>Logout</q-item-section>
            </q-item>
          </q-list>
        </q-menu>
      </q-toolbar>
    </q-header>
    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-list>
        <q-item clickable to="/">
          <q-item-section avatar><q-icon name="home" /></q-item-section>
          <q-item-section>Home</q-item-section>
        </q-item>
        <q-item clickable to="/tree">
          <q-item-section avatar><q-icon name="account_tree" /></q-item-section>
          <q-item-section>Family Tree</q-item-section>
        </q-item>
        <q-item clickable to="/members">
          <q-item-section avatar><q-icon name="group" /></q-item-section>
          <q-item-section>Members</q-item-section>
        </q-item>
        <q-item clickable to="/about">
          <q-item-section avatar><q-icon name="info" /></q-item-section>
          <q-item-section>About</q-item-section>
        </q-item>
      </q-list>
    </q-drawer>
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useAuthStore } from '../stores/auth'
import { useQuasar } from 'quasar'
import { useRouter } from 'vue-router'

const $q = useQuasar()
const authStore = useAuthStore()
const router = useRouter()

const leftDrawerOpen = ref(false)
const showProfileMenu = ref(false)
const user = computed(() => authStore.user)

const logout = async () => {
  await authStore.logout()
  router.push('/')
  $q.notify({ type: 'positive', message: 'Logged out successfully' })
}
</script>

<style scoped>
.navbar-glass {
  background: rgba(255, 255, 255, 0.85);
  backdrop-filter: blur(8px);
  box-shadow: 0 2px 16px rgba(25, 118, 210, 0.08);
}
</style>
