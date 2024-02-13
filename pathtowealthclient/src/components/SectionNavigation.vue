<template>
    <div class="overlay-component">
        <el-button class="el-nav-button" @click="scrollToSection('about')">
            <el-icon color="#e5eaf3"><InfoFilled /></el-icon>
        </el-button>
        <el-button class="el-nav-button" @click="scrollToSection('form')">
            <el-icon color="#e5eaf3"><Edit /></el-icon>
        </el-button>
        <el-button class="el-nav-button" @click="scrollToSection('investment')">
            <el-icon color="#e5eaf3"><Money /></el-icon>
        </el-button>
        <el-button class="el-nav-button" @click="scrollToSection('withdrawal')">
            <el-icon color="#e5eaf3"><Wallet /></el-icon>
        </el-button>
        <el-button class="el-nav-button" @click="scrollToSection('contact')">
            <el-icon color="#e5eaf3"><Phone /></el-icon>
        </el-button>
    </div>
</template>

<script lang="ts">
import { ElButton, ElIcon } from 'element-plus';

export default {
    components: {
        ElButton,
        ElIcon,
    },
    methods: {
        scrollToSection(sectionId: string) {
            const section = document.getElementById(sectionId);
            if (section) {
                section.scrollIntoView({ behavior: 'smooth' });
            }
        },
    },
};
</script>
<style lang="scss">
.overlay-component {
    position: absolute;
    width: 400px;
    top: 700px;
    left: 50%;
    transform: translateX(-50%);
    height: 50px;
    z-index: 1;
    background-color: rgba(0, 0, 0, 0.2);
    backdrop-filter: blur(10px);
    border-radius: 99rem;
    border-width: 0px;
    box-shadow: inset 0px 2px 8px rgba(0, 0, 0, 0.2);
    display: flex;
    justify-content: space-between;

}

.el-nav-button {
  color: var(--ep-text-color);
  text-align: center;
  border-radius: 99rem;
  transition: background-color 0.3s ease;
  height: 3rem;
  font-size: inherit;
  width: 1rem;
  border-width: 0px;
  height: 100%;
  width: 25%;
  padding: 0% 0% 0% 0%; 
  margin: 0% 0% 0% 0%;
  background-color: var(--ep-background-color);
  &:hover {
    background-color: transparent;
    backdrop-filter: blur(3px);
  }
  &:focus {
    background-color: transparent;
    backdrop-filter: blur(3px);
  }
}

@media (max-width: 455px) {
    .overlay-component {
        width: 240px;
    }
}
</style>