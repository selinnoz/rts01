Bu proje, Unity oyun motoru ve Unity ML-Agents Toolkit kullanılarak geliştirilmiş, gerçek zamanlı strateji (RTS) ortamında karar verebilen bir yapay zekâ ajanı (CommanderAgent) eğitmek için tasarlanmıştır. Ajan, kaynak yönetimi, birim üretimi ve görev atama gibi stratejik görevleri pekiştirmeli öğrenme (Reinforcement Learning - RL) ile öğrenmektedir.

## 🚀 Özellikler

- Unity ML-Agents ile PPO algoritması kullanılarak RL eğitimi
- Komutan ajanı sadece yüksek seviyeli kararları alır
- Villager (köylü) birimleri çevresel kaynakları (örneğin Wood) kendisi algılayarak hareket eder
- UI üzerinden kaynakları takip edebilir ve birim üretebilirsiniz
- Oyun ortamı: food ve wood üretimi, asker/okçu üretimi

## 🔧 Gereksinimler

### Unity Tarafı:
- Unity Editor (önerilen: **2022.3 LTS**)
- Unity ML-Agents (Yüklü olmalı, Package Manager üzerinden eklenebilir)

### Python Tarafı (RL Eğitimi için):
- Python 3.10+
- ML-Agents Toolkit

```bash
# Conda ortamı oluşturma
conda create -n mlagents python=3.10.12
conda activate mlagents

# Gerekli paketleri yükleme
conda install numpy=1.23.5
pip3 install torch~=2.2.1 --index-url https://download.pytorch.org/whl/cu121

# Start Python to verify installation
python
import torch
import numpy
print(torch.__version__)
print(numpy.__version__)
exit()

# Clear the terminal (optional)
clear

# Change directory to ML-Agents folder
cd D:\Unity\ml-agents

# Install ML-Agents from the local source files
python -m pip install ./ml-agents-envs
python -m pip install ./ml-agents

# Check ML-Agents installation
mlagents-learn --help

🛠️ Kurulum
Bu projeyi GitHub üzerinden klonlayın:

bash
Kopyala
Düzenle
git clone https://github.com/selinnoz/rts01
cd rts01
Unity ile rts01 klasörünü açın.

Gamescene sahnesini açın (Assets/Scenes/Gamescene.unity).

Unity'de sahneyi çalıştırmadan önce Behavior Parameters içeren ajan nesnesinin Behavior Name kısmının config.yaml ile uyumlu olduğundan emin olun.

Python terminalinde eğitimi başlatın:

bash
Kopyala
Düzenle

(mlagents) D:\>
mlagents-learn "D:\Unity\rts01\config.yaml" --run-id=RTS_Run001 --force
Unity'de sahneyi Play butonuna basarak çalıştırın. Eğitim otomatik olarak başlayacaktır.
