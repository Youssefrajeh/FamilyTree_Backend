const routes = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/IndexPage.vue') },
      { path: 'signup', component: () => import('pages/SignUpPage.vue') },
      { path: 'profile', component: () => import('pages/UserProfile.vue') },
      { path: 'members', component: () => import('pages/MembersPage.vue') },
      { path: 'about', component: () => import('pages/AboutPage.vue') },
      { path: 'tree', component: () => import('pages/FamilyTreePage.vue') },
      { path: 'add-member', component: () => import('pages/AddMemberPage.vue') },
    ]
  },
  // Always leave this as last one,
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
]

export default routes
